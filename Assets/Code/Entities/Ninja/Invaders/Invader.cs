using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public abstract class Invader : NPC
{
    private const float CHECK_ENEMY = 2.5F;

    protected List<Item> LootTable;
    protected List<int> LootAmount;
    protected List<float> LootChance;

    public float enemyFindTimer = 0;
    private LivingEntity last;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();

        InitLootTable();
        SetupLootTable();
        SetupInventory();

        CheckLootTable();
    }

    protected override void Update()
    {
        base.Update();

        if (PhotonNetwork.IsMasterClient)
        {
            enemyFindTimer += Time.deltaTime;
            if (enemyFindTimer > CHECK_ENEMY)
            {
                enemyFindTimer = 0;
                if (FindTarget(out LivingEntity opportunity))
                    if (last == null || GetPriority(opportunity) < GetPriority(last))
                    {
                        last = opportunity;
                        OverrideJob(new KillJob(this, opportunity));
                    }
            }
        }
    }

    #endregion

    protected override Job FindJob()
    {
        LivingEntity target;
        if (FindTarget(out target))
        {
            this.last = target;
            return new KillJob(this, target);
        }
        return new IdleJob(this);
    }

    protected bool FindTarget(out LivingEntity target)
    {
        target = null;
        foreach (LivingEntity enemy in LivingEntity.GetAllies())
        {
            if (ReferenceEquals(target, null))
            {
                target = enemy;
                continue;
            }
           
            if (GetPriority(target) > GetPriority(enemy))
            {
                target = enemy;
            }
        }
        return target != null;
    }

    protected float GetPriority(LivingEntity entity)
    {
        return GetDistance(entity) * (entity.IsVillager() ? 1.25F : 1F);
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


    private void InitLootTable()
    {
        this.LootChance = new List<float>();
        this.LootAmount = new List<int>();
        this.LootTable = new List<Item>();
    }

    protected abstract void SetupLootTable();

    protected abstract void SetupInventory();


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

    public override void OnDamage(LivingEntity source, float damage)
    {
        base.OnDamage(source, defend ? damage / 2 : damage);
    }



    #endregion
}
