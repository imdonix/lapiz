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

    //Components
    protected CharacterController characterController;

    protected Head head;
    protected Arms arms;
    protected Legs legs;
    protected Body body;

    //Inputs
    protected Vector3 direction = Vector3.zero;
    protected bool jump = false;
    protected bool sprint = false;
    protected Vector2 mouse = Vector2.zero;
    protected bool[] slots = new bool[SLOTS];
    protected bool[] seals = new bool[HANDSEALS];
    protected bool attack = false;
    protected bool cast = false;

    private float jumpTimer = 0;

    #region UNITY

    protected virtual void Start() {}

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();

        head = GetComponentInChildren<Head>();
        arms = GetComponentInChildren<Arms>();
        body = GetComponentInChildren<Body>();
        legs = GetComponentInChildren<Legs>();
    }

    protected virtual void Update()
    {
        if (photonView.IsMine)
        {
            UpdateStats();
            PassInteraction();
            UpdateTimers();
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
            Move();
    }

    #endregion

    #region PUBLIC

    public Body GetBody() 
    { 
        return body; 
    }

    public Head GetHead()
    {
        return head;
    }

    public float GetChakra()
    {
        return chakra;
    }

    public void SpendChakra(float chakra)
    {
        this.chakra -= chakra;
        if (this.chakra < 0) this.chakra = 0;
    }

    public Vector3 GetLookDirection()
    {
        return head.transform.forward;
    }

    public void Cast(Jutsu jutsu)
    {
        Jutsu casted = PhotonNetwork.Instantiate(jutsu.name, Vector3.zero, Quaternion.identity).GetComponent<Jutsu>();
        casted.AttachProperties(this);
        casted.Cast(this, head.transform.position + body.transform.forward, head.transform.forward);
    }

    #endregion

    #region PRIVATE

    private void UpdateStats()
    {
        health += healthRegen * Time.deltaTime;
        chakra += chakraRegen * Time.deltaTime;

        if (health > maxHealth) health = maxHealth;
        if (chakra > maxChakra) chakra = maxChakra;
    }

    private void PassInteraction()
    {
        if (cast) arms.CastJutsu();

        for (int i = 0; i < SLOTS; i++)
            if (slots[i])
            {
                arms.Swap(i - 1);
                return;
            }

        for (int i = 0; i < HANDSEALS; i++)
            if (seals[i])
            {
                arms.ShowSeal((HandSeal)i);
                return;
            }

        if (attack) arms.Attack();
    }


    private void UpdateTimers()
    {
        jumpTimer -= Time.deltaTime;
    }

    #endregion


    #region PROTECTED

    protected void Move()
    {
        if (characterController.isGrounded && jump) jumpTimer = JumpTime;

        Vector3 jumpForce = (jumpTimer > 0) ? transform.up * (Mathf.Cos((jumpTimer / JumpTime) * Mathf.PI + Mathf.PI) + 1) / 2 : Vector3.zero;
        Vector3 realDirection = (body.transform.rotation * direction);

        characterController.Move(((jumpForce * JumpSpeed) + (realDirection * (sprint ? 1.35F : 1) * Speed) + (Physics.gravity)) * Time.deltaTime);
    }

    protected abstract void Die();

    #endregion

    #region SERIALIZATION

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            this.health = (float)stream.ReceiveNext();
            this.chakra = (float)stream.ReceiveNext();
            arms.SetSlot((int)stream.ReceiveNext());
        }
        else
        {
            stream.SendNext(health);
            stream.SendNext(chakra);
            stream.SendNext(arms.GetSlot());
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