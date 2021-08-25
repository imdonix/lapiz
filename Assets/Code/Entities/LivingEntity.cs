using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class LivingEntity : Entity, IDamagable
{
    [Header("LivingEntity")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float maxChakra;
    [SerializeField] protected float health;
    [SerializeField] protected float chakra;
    [SerializeField] protected float healthRegen;
    [SerializeField] protected float chakraRegen;


    public static int SLOTS = 3;
    public static int HANDSEALS = 3;

    [Header("- Properties -")]
    [SerializeField] public float Speed;
    [SerializeField] public float JumpTime;
    [SerializeField] public float JumpSpeed;
    [SerializeField] public float Sensitivity;

    #region UNITY

    protected override void Update()
    {
        if (photonView.IsMine)
            UpdateStats();   
    }

    #endregion

    private void UpdateStats()
    {
        health += healthRegen * Time.deltaTime;
        chakra += chakraRegen * Time.deltaTime;

        if (health > maxHealth) health = maxHealth;
        if (chakra > maxChakra) chakra = maxChakra;
    }

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
        photonView.RPC("OnDamage", photonView.Owner, source.photonView.GetInstanceID(), damage);
    }

    [PunRPC]
    public void OnDamage(int id, float damage)
    {
        health -= damage;
        if (health < 0) Die();
    }

    #endregion


}