using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Invader : Ninja
{

    private const float CHECK_ENEMY = 2.5F;
    private const float CHECK_PATH = 5F;
    private const float REACH = 0.25F;
    private const float GOFORIT = 8.5F;
    private const float TOOH = 5F;

    [Header("Invader")]
    [SerializeField] public List<Item> LootTable;
    [SerializeField] public List<int> LootAmount;
    [SerializeField] public List<float> LootChance;

    public LivingEntity target = null;
    public Path path = null;

    public float enemyFindTimer = 0;
    public float pathFindTimer = 0;
    public bool isSearchingPath = false;

    #region UNITY

    protected override void Awake()
    {
        CheckLootTable();

        base.Awake();

        legs.Claim();
        arms.Claim();
    }

    protected override void Update()
    {
        base.Update();

        if (!PhotonNetwork.IsMasterClient) return;

        FindTarget();
        PathFind();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!PhotonNetwork.IsMasterClient) return;

        MoveInvader();
    }

    #endregion

    private void MoveInvader()
    {
        if (!ReferenceEquals(target, null))
        {
            Vector3 highlessMe = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 highlessTarget = new Vector3(target.transform.position.x, 0, target.transform.position.z);
            Vector3 direction = (highlessTarget - highlessMe).normalized; 

            float heightDistance = Mathf.Abs(transform.position.y - target.transform.position.y);
            float airDistance = Vector3.Distance(highlessTarget, highlessMe);

            SetTargetLook(target.transform.position);

            if (airDistance < GOFORIT || ReferenceEquals(path, null) || heightDistance > TOOH)
            {
                path = null;

                if (airDistance > REACH * 6F)
                    MoveTorwards(direction);
                else
                    MoveTorwards(Vector3.zero);
            }
            else
            {
                Vector3 checkPoint = path.Current().GetRealPos();

                direction = (checkPoint - highlessMe).normalized;
                if (Vector3.Distance(checkPoint, highlessMe) < REACH)
                    if (path.HasNext())
                    {
                        path.Next();
                    }
                    else
                    {
                        direction = Vector3.zero;
                        pathFindTimer = CHECK_PATH * 2;
                    }
                
                MoveTorwards(direction);
            }

        }
        else
        {
            MoveTorwards(Vector3.zero);
            SetIdleLook();
        }
    }

    private void MoveTorwards(Vector3 direction)
    {
        if(!direction.AlmostEquals(Vector3.zero, 0.05F))
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        characterController.Move(((direction * (sprint ? 1.35F : 1) * Speed) + (Physics.gravity)) * Time.deltaTime);
        legs.Forward(direction.magnitude * Speed * Time.deltaTime, sprint, characterController.isGrounded);
    }

    private void SetTargetLook(Vector3 target)
    {
        head.transform.rotation = Quaternion.LookRotation(target - transform.position);
    }

    private void SetIdleLook()
    {
        head.transform.localRotation = Quaternion.identity;
    }

    private void FindTarget()
    {
        if (ReferenceEquals(target, null) || enemyFindTimer > CHECK_ENEMY)
        {
            enemyFindTimer = 0;

            foreach (LivingEntity enemy in LivingEntity.GetAllies())
            {

                if (ReferenceEquals(target, null))
                {
                    target = enemy;
                    continue;
                }
           
                if (GetInvaderDistance(target) > GetInvaderDistance(enemy))
                {
                    target = enemy;
                    pathFindTimer = 0;
                }
            }
        }

        enemyFindTimer += Time.deltaTime;
    }

    private void PathFind()
    {
        if (target != null && !isSearchingPath && pathFindTimer > CHECK_PATH)
        {
            pathFindTimer = 0;
            
            PathFinder finder = World.Loaded.GetPathFinder();
            Vector2Int from = finder.GetGridPosition(transform.position);
            Vector2Int to = finder.GetGridPosition(target.transform.position);

            isSearchingPath = true;
            StartCoroutine(PathFindResoult(finder.Request(from, to)));
        }

        pathFindTimer += Time.deltaTime;
    }

    private IEnumerator PathFindResoult(PathFindRequest req)
    {
        yield return new WaitUntil(req.IsDone);
        isSearchingPath = false;

        if (req.IsSuccessful())
            path = req.GetPath();
        else
            path = null;
    }

    private float GetInvaderDistance(LivingEntity entity)
    { 
        return Vector3.Distance(transform.position, entity.transform.position) * (entity.IsVillager() ? 1.25F : 1F);
    }

    private void CheckLootTable() 
    {
        if (LootTable.Count != LootChance.Count) 
            Debug.LogError(string.Format("Loot table is not correct (chance) {0}", name));
        if (LootTable.Count != LootAmount.Count)
            Debug.LogError(string.Format("Loot table is not correct (amount) {0}", name));
    }

    private void GenerateLoot()
    {
        for (int i = 0; i < LootTable.Count; i++)
            for (int y = 0; y < LootAmount[i]; y++)
                if (Random.Range(0, 1F) < LootChance[i])
                    PhotonNetwork.Instantiate(LootTable[i].name, GetRandomDropLocation(), Quaternion.identity);
    }

    private Vector3 GetRandomDropLocation()
    {
        return transform.position + new Vector3(Random.Range(-1F, 1F), 1F, Random.Range(-1F, 1F));
    }

    #region NINJA

    public override bool IsAlly()
    {
        return false;
    }

    public override bool IsVillager()
    {
        return false;
    }

    protected override void Die()
    {
        GenerateLoot();
        PhotonNetwork.Destroy(photonView);
    }

    #endregion
}
