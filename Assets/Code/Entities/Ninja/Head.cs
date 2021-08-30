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

    private Camera cam = null;
    private Village badge = Village.None;

    private IInteractable currentInteractable = null;

    public Camera AttachCamera()
    {
        cam = cameraContainer.AddComponent<Camera>();
        cam.cullingMask = 0b0000000010111111;
        cam.fieldOfView = Settings.Instance.FOV;
        return cam;
    }

    public Village GetBadge()
    {
        return badge;
    }

    public IInteractable GetCurrentInteractable()
    {
        return currentInteractable;
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

    private void HideAllBadge()
    {
        palmHolder.SetActive(false);
        foreach (GameObject badge in villageBadges)
            badge.SetActive(false);
    }

}
