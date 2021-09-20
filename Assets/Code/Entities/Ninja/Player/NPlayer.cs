using Photon.Pun;
using UnityEngine;

public class NPlayer : Ninja
{
    private Vector3 camRot = Vector3.zero;
    private Vector2 mouse = Vector2.zero;
    private bool interact = false;
    private bool throwAway = false;
    private bool consume = false;
    private bool ready = false;
    private bool itemLib = false;

    #region UNITY


    protected override void Start()
    {
        base.Start();

        if(photonView.IsMine)
        {
            Claim();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            Story.Loaded.population.Populate(this);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (photonView.IsMine)
        {
            ReadInputs();
            PassInteraction();
            MoveCamera();
            UpdateGUI();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (photonView.IsMine)
            Move();
    }
  
    #endregion

    #region PUBLIC

    public void TakeControll()
    {
        this.name = "-Local Player-";
        MaskBodyParts();
        Camera.SetupCurrent(head.AttachCamera());
        HUD.Instance.SwitchPlayerOverlay();
    }

    public override bool IsAlly()
    {
        return true;
    }

    public override bool IsVillager()
    {
        return false;
    }

    #endregion

    protected void Move()
    {
        if (characterController.isGrounded && jump) jumpTimer = JumpTime;

        Vector3 jumpForce = (jumpTimer > 0) ? transform.up * (Mathf.Cos((jumpTimer / JumpTime) * Mathf.PI + Mathf.PI) + 1) / 2 : Vector3.zero;
        Vector3 realDirection = (body.transform.rotation * new Vector3(direction.x, 0, direction.y));

        characterController.Move(((jumpForce * JumpSpeed) + (realDirection * (sprint ? 1.35F : 1) * Speed) + (Physics.gravity)) * Time.deltaTime);
    }

    private void ReadInputs()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        jump = Input.GetKey(Settings.Instance.Jump);
        sprint = Input.GetKey(Settings.Instance.Sprint);
        mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        attack = Input.GetMouseButton(0);
        defend = Input.GetMouseButton(1);
        cast = Input.GetKeyDown(Settings.Instance.Cast);
        interact = Input.GetKeyDown(Settings.Instance.Interact);
        throwAway = Input.GetKeyDown(Settings.Instance.Throw);
        consume = Input.GetKeyDown(Settings.Instance.Consume);
        ready = Input.GetKeyDown(Settings.Instance.Ready);
        itemLib = Input.GetKeyDown(Settings.Instance.ItemLib);
        for (int i = 0; i < SLOTS; i++) slots[i] = Input.GetKeyDown(KeyCode.Alpha0 + i);
        for (int i = 0; i < HANDSEALS; i++) seals[i] = Input.GetKeyDown(KeyCode.F1 + i);
    }

    private void PassInteraction()
    {
        head.Interact(interact);

        if (consume) arms.Consume();

        if (throwAway) arms.ThrowAway();

        if (cast) arms.CastJutsu();

        if (ready) Story.Loaded.SendReady();

        if (itemLib) HUD.Instance.ToggleItemLibrary();

        for (int i = 0; i < SLOTS; i++)
            if (slots[i])
            {
                if (i == 0) arms.Swap(-1);
                Swap((ToolType)i - 1);
                return;
            }

        for (int i = 0; i < HANDSEALS; i++)
            if (seals[i])
            {
                arms.ShowSeal((HandSeal)i);
                return;
            }

        if (attack && !defend) arms.Attack();
    }

    private void UpdateGUI()
    {
        HUD.Instance.Action.Show(head.GetCurrentInteractable(), arms.GetItemInHand());
        HUD.Instance.LiveState.UpdateStatus(health, maxHealth, chakra, maxChakra);
    }

    private void MaskBodyParts()
    {
        foreach (Transform trans in head.GetComponentsInChildren<Transform>(true))
            trans.gameObject.layer = 6;
        head.gameObject.layer = body.gameObject.layer = 6;
    }

    private void Claim()
    {
        RequestBadge(Village.Mater);
    }

    private void MoveCamera()
    {
        camRot.x -= mouse.y * Sensitivity;
        camRot.x = Mathf.Clamp(camRot.x, -80, 75);
        camRot.y += mouse.x * Sensitivity;
        transform.rotation = Quaternion.Euler(new Vector3(0, camRot.y, 0));
        head.transform.localRotation = Quaternion.Euler(new Vector3(camRot.x, 0, 0));
    }

    protected override void Die()
    {
        DropInventoryItems();
        PhotonNetwork.Destroy(photonView);
        Story.Loaded.population.RegisterDead(this);

        World.Loaded.TakeFreeCam();
    }

    public override void Equip(Tool item)
    {
        if (inventory.Equip(item, out Tool old))
        {
            Item spawned = PhotonNetwork.Instantiate(old.name, transform.position + transform.forward, Quaternion.identity).GetComponent<Item>();
            arms.ThrowAway(spawned);
        }
        Swap(item.GetToolType());
    }

    public override void OnDamage(LivingEntity source, float damage)
    {
        if (canDefend) return;

        base.OnDamage(source, damage);
    }
}

