using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviourPun, IPunObservable
{
    private const float MIN = 0.25F;
    private const float OPTIMAL_SPAWN = 5;
    private const int MAX_VILLAGER_SPAWN = 2;

    [Header("Settings")]
    [SerializeField] private float idlePhaseTime;
    [SerializeField] private float attackPhaseTime;
    [SerializeField] private Wave[] Waves;

    [SerializeField] private bool attacking = false;
    [SerializeField] private float countDown = 0;
    [SerializeField] private int remaining = 0;

    [Header("Village")]
    [SerializeField] public Villager VillagerPref;

    private int readyCounter = 0;

    private int index = 0;
    private bool ready = false;
    private bool end = false;

    public static Story Loaded;
    public Population population;

    #region UNITY

    private void Awake()
    {
        Loaded = this;

        if (PhotonNetwork.IsMasterClient)
        {
            StartGame();
        }
    }

    private void Update()
    {
        if (end) return;

        if (PhotonNetwork.IsMasterClient)
        {
            remaining = attacking ? LivingEntity.GetEnemies().Count : readyCounter;
            countDown -= Time.deltaTime;

            if (countDown < 0 || ((remaining == 0) && attacking))
            {
                if (attacking)
                    IdlePhase();
                else
                    AttackPhase();
            }

            population.AwakeMangekyo();
            end = CheckEndConditions();

            if (end)
                SendGameOver();
            else
                HUD.Instance.Story.Show(attacking, countDown, remaining, ready);
        }
    }

    #endregion

    private void StartGame()
    {
        population = new Population(this);
        IdlePhase();
    }

    private void IdlePhase()
    {
        countDown = idlePhaseTime;
        attacking = false;

        int missingVillagers = GetIdealVillagerCount() - population.GetVillagerCount();
        Debug.Assert(missingVillagers > 0);        
        SpawnVillager(missingVillagers);
    }

    private void AttackPhase()
    {
        readyCounter = 0;
        ready = false;
        countDown = attackPhaseTime;
        attacking = true;
        SpawnNextWave();
    }


    public void SpawnVillager(int missingVillagers)
    {
        StartCoroutine(Spawn());
        IEnumerator Spawn()
        {
            for (int i = 0; i < missingVillagers && i < MAX_VILLAGER_SPAWN; i++)
            {
                yield return new WaitForSeconds(OPTIMAL_SPAWN / 2);
                if (end) yield break;

                PhotonNetwork.InstantiateRoomObject(
                    VillagerPref.name,
                    World.Loaded.GetVillagerSpawnPoint(),
                    Quaternion.identity);
            }
        }
    }

    private void SpawnNextWave()
    {
        StartCoroutine(Spawn());
        IEnumerator Spawn()
        {
            index++;
            int current = index % Waves.Length;
            int level = index / Waves.Length;

            Wave wave = Waves[current];
            float wait = OPTIMAL_SPAWN / wave.Entities.Length;
            foreach (LivingEntity entity in wave.Entities)
            {
                LivingEntity ent = PhotonNetwork.InstantiateRoomObject(entity.name,
                    World.Loaded.GetAEnemySpawnPosition(),
                    Quaternion.identity).GetComponent<LivingEntity>();
                ent.LevelUp(level);
                yield return new WaitForSeconds(Mathf.Max(wait, MIN));
                if (end) yield break;
            }
        }
    }


    private int GetIdealVillagerCount()
    {
        return 2 + Mathf.Min((index / 3), 5);
    }

    private bool CheckEndConditions()
    {
        return population.IsDead();
    }

    #region SERIALIZATION

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            this.countDown = (float)stream.ReceiveNext();
            this.attacking = (bool)stream.ReceiveNext();
            this.remaining = (int)stream.ReceiveNext();
            this.ready = attacking ? false : this.ready;
            HUD.Instance.Story.Show(attacking, countDown, remaining, ready);
        }   
        else
        {
            stream.SendNext(countDown);
            stream.SendNext(attacking);
            stream.SendNext(remaining);
        }
    }

    #endregion

    #region PUN

    [PunRPC]
    public void OnReady(bool ready)
    {
        if (attacking) return;

        readyCounter += ready ? 1 : -1;
        readyCounter = Mathf.Max(readyCounter, 0);
        if (readyCounter >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            AttackPhase();
        }
    }

    public void SendReady()
    {
        ready = !ready;
        photonView.RPC("OnReady", RpcTarget.MasterClient, ready);
    }

    [PunRPC]
    public void OnGameOver(int level)
    {
        HUD.Instance.SwitchGameOverOverlay(level);
    }

    public void SendGameOver()
    {
        photonView.RPC("OnGameOver", RpcTarget.AllBuffered, index);
    }

    #endregion
}