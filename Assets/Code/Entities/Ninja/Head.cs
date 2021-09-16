using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [Header("Head")]
    [SerializeField] private GameObject cameraContainer;
    [SerializeField] private GameObject palmHolder;
    [SerializeField] private GameObject[] villageBadges;
    [SerializeField] private Renderer[] eyes;
    [SerializeField] private Material[] eyeTypes;

    private Ninja owner;

    private Camera cam = null;
    private Village badge = Village.None;
    private EyeType eye = EyeType.Normal;
    private IInteractable currentInteractable = null;

    #region UNITY

    private void Awake()
    {
        owner = gameObject.GetComponentInParent<Ninja>();
        InitEyes();
    }

    #endregion

    public Camera AttachCamera()
    {
        cam = cameraContainer.AddComponent<Camera>();
        cam.cullingMask = 0b0000000010111111;
        cam.fieldOfView = Settings.Instance.FOV;
        return cam;
    }

    public IInteractable GetCurrentInteractable()
    {
        return currentInteractable;
    }

    public void Interact(bool interact)
    {
        if (ReferenceEquals(cam, null)) return;
        IInteractable interactable = null;
        if (GetCameraTarget(ref interactable))
        {
            currentInteractable = interactable;
            if (interact && interactable.CanInteract())
                interactable.Interact(GetComponentInParent<NPlayer>());
        }
        else
            currentInteractable = null;
    }

    public bool GetCameraTarget<T>(ref T hit)
    {
        Ray ray = cam.ViewportPointToRay(Vector3.one * .5F);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit))
        {
            T component = rayHit.collider.GetComponent<T>();
            if (!ReferenceEquals(component, null))
            {
                hit = component;
                return true;
            }
        }
        return false;
    }


    #region BADGE

    public Village GetBadge()
    {
        return badge;
    }

    public void SetBadge(Village badge)
    {
        if (this.badge == badge) return;

        HideAllBadge();
        if (!ReferenceEquals(badge, Village.None))
        {
            palmHolder.SetActive(true);
            villageBadges[(int)badge - 1].SetActive(true);
        }

        this.badge = badge;
    }

    private void HideAllBadge()
    {
        palmHolder.SetActive(false);
        foreach (GameObject badge in villageBadges)
            badge.SetActive(false);
    }


    #endregion


    #region EYE

    public EyeType GetEye()
    {
        return eye;
    }

    private void InitEyes()
    {
        if (Enum.GetNames(typeof(EyeType)).Length != eyeTypes.Length)
            throw new Exception("Eyes materials missing");
        owner.RequestEye(eye);
    }

    public bool HasEye(EyeType type)
    {
        return (int)eye >= (int)type;
    }

    public void AwakeEye(EyeType type)
    {
        if ((int)type < (int)eye) return;
        owner.RequestEye(type);
    }

    public void SetEye(EyeType type)
    {
        if (!ReferenceEquals(type, eye))
        {
            this.eye = type;
            foreach (Renderer ren in eyes)
            {
                ren.material = eyeTypes[(int)eye];
                if (eye == EyeType.Normal || eye == EyeType.Rinnegan)
                    ren.material.color = DNA.GetEye(owner.GetDNA(), eye);
            }

        }
    }

    #endregion
}
