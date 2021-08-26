using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [Header("Head")]
    [SerializeField] private GameObject cameraContainer;
    [SerializeField] private GameObject palmHolder;
    [SerializeField] private GameObject[] villageBadges;


    private Village badge = Village.None;

    public Camera AttachCamera()
    {
        Camera cam = cameraContainer.AddComponent<Camera>();
        cam.cullingMask = 0b0000000010111111;
        cam.fieldOfView = Settings.Instance.FOV;
        return cam;
    }

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



}
