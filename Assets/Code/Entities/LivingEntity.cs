using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : Entity, IDamagable
{

    private const float CHAKRA_REGEN_SEC = 10F;
    private const float HEALTH_REGEN_SEC = 60F;

    private const float HEALTH_INC_LEVEL = 1.35F;
    private const float CHAKRA_INC_LEVEL = 2F;


    private static List<LivingEntity> Allies = new List<LivingEntity>();
    private static List<LivingEntity> Enemies = new List<LivingEntity>();



    [Header("LivingEntity")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float maxChakra;

    [Header("Properties")]
    [SerializeField] protected GameObject blood;

    [Header("View")]
    [SerializeField] protected float health;
    [SerializeField] protected float chakra;
    [SerializeField] protected int level;

    protected int dna;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();

        InitStats();

        if (IsAlly())
            Allies.Add(this);
        else
            Enemies.Add(this);

        if (photonView.IsMine)
            InitDNA();
    }

    protected override void Update()
    {
        if (photonView.IsMine)
            UpdateStats();   
    }

    private void OnDestroy()
    {
        if (IsAlly())
            Allies.Remove(this);
        else
            Enemies.Remove(this);
    }

    #endregion

    private void UpdateStats()
    {
        health += (maxHealth / HEALTH_REGEN_SEC) * Time.deltaTime;
        chakra += (maxChakra / CHAKRA_REGEN_SEC) * Time.deltaTime;

        if (health > maxHealth) health = maxHealth;
        if (chakra > maxChakra) chakra = maxChakra;
    }

    public override void Update(EntityTag entityTag)
    {
        base.Update(entityTag);
        entityTag.SetBadge(string.Format("{0} lvl", GetLevel()));
        entityTag.SetChakra(this);
        entityTag.SetHealth(this);
    }

    private void InitStats()
    {
        this.health = maxHealth;
        this.chakra = maxChakra;
    }

    private void InitDNA()
    {
        this.dna = DNA.GetRandom();
        photonView.RPC("ShareDNA", RpcTarget.OthersBuffered, this.dna);
    }

    public void IncreaseMaxChakra(float amount)
    {
        maxChakra += amount;
        chakra += amount;
    }

    public void IncreaseMaxHP(float amount)
    {
        maxHealth += amount;
        health += amount;
    }

    public void SpendChakra(float chakra)
    {
        this.chakra -= chakra;
        if (this.chakra < 0) this.chakra = 0;
    }

    public float GetMaxHP()
    {
        return maxHealth;
    }

    public float GetMaxChakra()
    {
        return maxChakra;
    }

    public float GetHP()
    {
        return health;
    }

    public float GetChakra()
    {
        return chakra;
    }

    public int GetLevel()
    {
        return level;
    }

    public virtual void LevelUp(int level)
    {
        int corrected = level + 1;
        this.maxHealth *= HEALTH_INC_LEVEL * corrected;
        this.maxChakra *= CHAKRA_INC_LEVEL * corrected;
        this.level = level;
        InitStats();
    }

    /// <summary>
    /// Unique DNA for each unit
    /// </summary>
    public int GetDNA() { return dna; }

    /// <summary>
    /// Is Entity a aly type
    /// </summary>
    /// <returns></returns>
    public abstract bool IsAlly();


    /// <summary>
    /// Is Entity a villager type
    /// </summary>
    /// <returns></returns>
    public abstract bool IsVillager();


    /// <summary>
    /// Only the owner called
    /// </summary>
    protected abstract void Die();

    /// <summary>
    /// Return the position where the hunter will look, word pos
    /// </summary>
    public abstract Vector3 GetTargetedLookPosition();

    /// <summary>
    /// Alert the target to danger.
    /// </summary>
    public virtual void Alert(LivingEntity source) {}

    public virtual void OnDamage(LivingEntity source, float damage)
    {
        SplashBlood();
        health -= damage;
        if (health < 0) 
            Die();

    }

    #region SERIALIZATION

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            this.health = (float)stream.ReceiveNext();
            this.chakra = (float)stream.ReceiveNext();
        }
        else
        {
            stream.SendNext(health);
            stream.SendNext(chakra);
        }
    }

    #endregion

    #region RPC

    public void Damage(LivingEntity source, float damage)
    {
        if ((source.IsAlly() && IsAlly()) || (!source.IsAlly() && !IsAlly())) return;

        photonView.RPC("OnDamageRPC", photonView.Owner, source.photonView.ViewID, damage);
    }


    public void SplashBlood()
    {
        photonView.RPC("OnSplashBlood", RpcTarget.All);
    }

    [PunRPC]
    public void OnDamageRPC(int id, float damage)
    {
        OnDamage(LivingEntity.Get(id), damage);
    }

    [PunRPC]
    public void OnSplashBlood()
    {
        Instantiate(blood, transform.position + Vector3.up, Quaternion.identity);
    }

    [PunRPC]
    public void ShareDNA(int dna)
    {
        this.dna = dna;
    }

    #endregion

    #region STATIC

    public static List<LivingEntity> GetAllies()
    {
        return LivingEntity.Allies;
    }

    public static List<LivingEntity> GetEnemies()
    {
        return LivingEntity.Enemies;
    }

    public static LivingEntity Get(int id)
    {
        foreach (var item in Allies)
            if (item.photonView.ViewID == id)
                return item.GetComponent<LivingEntity>();
        foreach (var item in Enemies)
            if (item.photonView.ViewID == id)
                return item.GetComponent<LivingEntity>();
        return null;
    }

    #endregion
}