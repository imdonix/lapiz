using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class Ninja : LivingEntity
{

    [Header("Ninja")]
    [SerializeField] public float Speed;
    [SerializeField] public float JumpTime;
    [SerializeField] public float JumpSpeed;
    [SerializeField] public float Sensitivity;

    protected CharacterController characterController;

    protected Head head;
    protected Arms arms;
    protected Legs legs;
    protected Body body;

    protected Vector2 direction = Vector2.zero;
    protected bool jump = false;
    protected bool sprint = false;
    protected bool[] slots = new bool[SLOTS];
    protected bool[] seals = new bool[HANDSEALS];
    protected bool attack = false;
    protected bool cast = false;

    private float jumpTimer = 0;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();

        characterController = GetComponent<CharacterController>();

        head = GetComponentInChildren<Head>();
        arms = GetComponentInChildren<Arms>();
        body = GetComponentInChildren<Body>();
        legs = GetComponentInChildren<Legs>();

        if (photonView.IsMine)
        {
            arms.Claim();
            legs.Claim();
        }
    }

    protected override void Update()
    {
        base.Update();

        if (photonView.IsMine)
        {
            UpdateTimers();
            MoveLegs();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (photonView.IsMine)
            Move();
    }

    #endregion

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

    public Vector3 GetVelocity()
    {
        return characterController.velocity;
    }

    public void Cast(Jutsu jutsu)
    {
        Jutsu casted = PhotonNetwork.Instantiate(jutsu.name, Vector3.zero, Quaternion.identity).GetComponent<Jutsu>();
        casted.Cast(this, head.transform.position + body.transform.forward, head.transform.forward);
    }

    public override void Teleport(Vector3 position)
    {
        characterController.enabled = false;
        transform.position = position;
        characterController.enabled = true;
    }

    public void PickUpItem(Item item)
    {
        arms.PickUpItem(item);
    }

    protected void Move()
    {
        if (characterController.isGrounded && jump) jumpTimer = JumpTime;

        Vector3 jumpForce = (jumpTimer > 0) ? transform.up * (Mathf.Cos((jumpTimer / JumpTime) * Mathf.PI + Mathf.PI) + 1) / 2 : Vector3.zero;
        Vector3 realDirection = (body.transform.rotation * new Vector3(direction.x, 0, direction.y));

        characterController.Move(((jumpForce * JumpSpeed) + (realDirection * (sprint ? 1.35F : 1) * Speed) + (Physics.gravity)) * Time.deltaTime);
    }

    public void MoveTorwards(Vector3 direction)
    {
        if (!direction.AlmostEquals(Vector3.zero, 0.05F))
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        characterController.Move(((direction * (sprint ? 1.35F : 1) * Speed) + (Physics.gravity)) * Time.deltaTime);
        legs.Forward(direction.magnitude * Speed * Time.deltaTime, sprint, characterController.isGrounded);
    }

    public void SetTargetLook(Vector3 target)
    {
        head.transform.rotation = Quaternion.LookRotation(target - transform.position);
    }

    public void SetIdleLook()
    {
        head.transform.localRotation = Quaternion.identity;
    }

    private void UpdateTimers()
    {
        jumpTimer -= Time.deltaTime;
    }

    private void MoveLegs() 
    {
        legs.Forward(direction.y * Speed * Time.deltaTime, sprint, characterController.isGrounded);
    }

    #region SERIALIZATION

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);

        if (stream.IsReading)
        {
            arms.SetSlot((int)stream.ReceiveNext());
            head.SetBadge((Village)stream.ReceiveNext());
        }
        else
        {
            stream.SendNext(arms.GetSlot());
            stream.SendNext(head.GetBadge());
        }
    }

    #endregion

}
