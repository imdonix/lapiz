using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class Ninja : LivingEntity
{
    private const float ROTATION = 90 * 2.5F;

    public static int SLOTS = 4;
    public static int HANDSEALS = 3;

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
    protected Inventory inventory;

    protected Vector2 direction = Vector2.zero;
    protected bool jump = false;
    protected bool sprint = false;
    protected bool[] slots = new bool[SLOTS];
    protected bool[] seals = new bool[HANDSEALS];
    protected bool attack = false;
    protected bool cast = false;

    protected    float jumpTimer = 0;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();

        characterController = GetComponent<CharacterController>();

        head = GetComponentInChildren<Head>();
        arms = GetComponentInChildren<Arms>();
        body = GetComponentInChildren<Body>();
        legs = GetComponentInChildren<Legs>();
        inventory = new Inventory();

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

    #endregion

    public Body GetBody()
    {
        return body;
    }

    public Head GetHead()
    {
        return head;
    }

    public Arms GetArms()
    {
        return arms;
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

    public void Swap(ToolType type)
    {
        Tool tool = inventory.GetTool(type);
        if (ReferenceEquals(tool, null))
        {
            arms.Swap(-1);
            return;
        }

        Weapon[] weapons = arms.GetEquipmentSlots();
        for (int i = 0; i < weapons.Length; i++)
            if (weapons[i].GetItemPref().Equals(tool))
            {
                if (arms.GetSlot() == i)
                    return;

                arms.Swap(i);
                return;
            }
    }

    public void MoveTorwards(Vector3 direction)
    {
        MoveBodyOnly(direction);
        characterController.Move(((direction * (sprint ? 1.35F : 1) * Speed) + (Physics.gravity)) * Time.deltaTime);
        legs.Forward(direction.magnitude * Speed * Time.deltaTime, sprint, characterController.isGrounded);
    }

    public void MoveBodyOnly(Vector3 direction)
    {
        if (!direction.AlmostEquals(Vector3.zero, 0.05F))
            SetBodyLook(Quaternion.LookRotation(direction, Vector3.up));
    }

    public void SetTargetLook(Vector3 target)
    {
        Quaternion tar = Quaternion.LookRotation(target - head.transform.position);
        SetHeadLook(tar);
    }

    public void SetIdleLook()
    {
        SetHeadLook(Quaternion.identity);
    }

    private void SetHeadLook(Quaternion tar)
    {
        Quaternion cur = head.transform.rotation;
        head.transform.rotation = Quaternion.RotateTowards(cur, tar, ROTATION * 2 * Time.deltaTime);
    }

    private void SetBodyLook(Quaternion tar)
    {
        Quaternion cur = body.transform.rotation;
        body.transform.rotation = Quaternion.RotateTowards(cur, tar, ROTATION * Time.deltaTime);
    }

    public override Vector3 GetTargetedLookPosition()
    {
        return body.transform.position;
    }

    private void UpdateTimers()
    {
        jumpTimer -= Time.deltaTime;
    }

    private void MoveLegs() 
    {
        legs.Forward(direction.y * Speed * Time.deltaTime, sprint, characterController.isGrounded);
    }

    public abstract void Equip(Tool item);

    protected Vector3 GetRandomDropLocation()
    {
        return transform.position + new Vector3(UnityEngine.Random.Range(-1.5F, 11.5F), 1F, UnityEngine.Random.Range(-1.5F, 1.5F));
    }

    protected void DropInventoryItems()
    {
        Item item = arms.GetItemInHand();
        if (!ReferenceEquals(item, null))
            PhotonNetwork.Instantiate(item.GetItemPref().name, GetRandomDropLocation(), Quaternion.identity);

        foreach (Tool tool in inventory.GetAll())
            if(!ReferenceEquals(tool, null))
                PhotonNetwork.Instantiate(tool.name, GetRandomDropLocation(), Quaternion.identity);
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
