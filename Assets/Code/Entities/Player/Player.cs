using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : LivingEntity
{

    private Vector3 camRot = Vector3.zero;

    #region UNITY

    protected override void Start()
    {
        base.Start();

        if(photonView.IsMine)
        {
            Claim();
        }
    }

    protected override void Update()
    {
        base.Update();

        if (photonView.IsMine)
        {
            ReadInputs();
            MoveCamera();
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

    #endregion

    private void ReadInputs()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        jump = Input.GetKey(KeyCode.Space);
        sprint = Input.GetKey(KeyCode.LeftShift);
        mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        attack = Input.GetMouseButton(0);
        cast = Input.GetKeyDown(KeyCode.E);
        for (int i = 0; i < SLOTS; i++) slots[i] = Input.GetKeyDown(KeyCode.Alpha0 + i);
        for (int i = 0; i < HANDSEALS; i++) seals[i] = Input.GetKeyDown(KeyCode.F1 + i);

        legs.Forward(direction.z * Speed * Time.deltaTime, sprint, characterController.isGrounded);
    }

    private void MaskBodyParts()
    {
        foreach (Transform trans in head.GetComponentsInChildren<Transform>(true))
            trans.gameObject.layer = 6;
        head.gameObject.layer = body.gameObject.layer = 6;
    }

    private void Claim()
    {
        legs.Claim();
        arms.Claim();
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
        Manager.Instance.FreeCamera.EnableFreeCam();
    }

}

