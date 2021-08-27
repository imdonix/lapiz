using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class LivingEntity : Entity, IDamagable
{

    private static List<LivingEntity> Allies = new List<LivingEntity>();
    private static List<LivingEntity> Enemies = new List<LivingEntity>();

    [Header("LivingEntity")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float maxChakra;
    [SerializeField] protected float health;
    [SerializeField] protected float chakra;
    [SerializeField] protected float healthRegen;
    [SerializeField] protected float chakraRegen;


    public static int SLOTS = 3;
    public static int HANDSEALS = 3;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();

        if (IsAlly())
            Allies.Add(this);
        else
            Enemies.Add(this);
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
        health += healthRegen * Time.deltaTime;
        chakra += chakraRegen * Time.deltaTime;

        if (health > maxHealth) health = maxHealth;
        if (chakra > maxChakra) chakra = maxChakra;
    }

    public virtual void Teleport(Vector3 position)
    {
        transform.position = position;
    }

    public abstract bool IsAlly();

    public abstract bool IsVillager();

    protected abstract void Die();

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

        photonView.RPC("OnDamage", photonView.Owner, source.photonView.GetInstanceID(), damage);
    }

    [PunRPC]
    public void OnDamage(int id, float damage)
    {
        health -= damage;
        if (health < 0) Die();
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

    #endregion
}