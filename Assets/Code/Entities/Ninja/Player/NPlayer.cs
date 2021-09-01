using Photon.Pun;
using UnityEngine;

public class NPlayer : Ninja
{
    private Vector3 camRot = Vector3.zero;
    private Vector2 mouse = Vector2.zero;
    private bool interact = false;
    private bool throwAway = false;
    private bool consume = false;

    #region UNITY

    protected override void Start()
    {
        base.Start();

        if(photonView.IsMine)
        {
            Claim();
            DebugPath();
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

    #endregion

    #region PUBLIC

    public void TakeControll()
    {
        this.name = "-Local Player-";
        MaskBodyParts();
        Camera.SetupCurrent(head.AttachCamera());
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

    private void DebugPath()
    {
        PathFinder finder = World.Loaded.GetPathFinder();
        Vector2Int start = finder.GetGridPosition(transform.position);
        Vector2Int end = finder.GetGridPosition(new Vector3(98, 0, 75));
        PathFindRequest req = finder.Request(start, end);
    }

    private void ReadInputs()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        jump = Input.GetKey(Settings.Instance.Jump);
        sprint = Input.GetKey(Settings.Instance.Sprint);
        mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        attack = Input.GetMouseButton(0);
        cast = Input.GetKeyDown(Settings.Instance.Cast);
        interact = Input.GetKeyDown(Settings.Instance.Interact);
        throwAway = Input.GetKeyDown(Settings.Instance.Throw);
        consume = Input.GetKeyDown(Settings.Instance.Consume);
        for (int i = 0; i < SLOTS; i++) slots[i] = Input.GetKeyDown(KeyCode.Alpha0 + i);
        for (int i = 0; i < HANDSEALS; i++) seals[i] = Input.GetKeyDown(KeyCode.F1 + i);
    }

    private void PassInteraction()
    {
        head.Interact(interact);

        if (consume) arms.Consume();

        if(throwAway) arms.ThrowAway();

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

    private void UpdateGUI()
    {
        HUD.Instance.Show(head.GetCurrentInteractable(), arms.GetItemInHand());
    }

    private void MaskBodyParts()
    {
        foreach (Transform trans in head.GetComponentsInChildren<Transform>(true))
            trans.gameObject.layer = 6;
        head.gameObject.layer = body.gameObject.layer = 6;
    }

    private void Claim()
    {
        head.SetBadge(Village.Mater);
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
        PhotonNetwork.Destroy(photonView);
    }
}

