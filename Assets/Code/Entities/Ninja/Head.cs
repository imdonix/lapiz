using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject cameraContainer;

    public Camera AttachCamera()
    {
        Camera cam = cameraContainer.AddComponent<Camera>();
        cam.cullingMask = 0b0000000000111111;
        cam.fieldOfView = Settings.Instance.FOV;
        return cam;
    }
}
