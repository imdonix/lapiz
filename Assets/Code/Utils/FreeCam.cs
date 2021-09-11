using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FreeCam : MonoBehaviour
{

    private const float movementSpeed = 20f;
    private const float fastMovementSpeed = 50f;
    private const float freeLookSensitivity = 3f;
    private const float zoomSensitivity = 10f;
    private const float fastZoomSensitivity = 50f;

    private bool active;

    private Camera cam;

    #region UNITY

    private void Awake()
    {
        this.cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!active) return;

        bool fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float movementSpeed = fastMode ? FreeCam.fastMovementSpeed : FreeCam.movementSpeed;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            transform.position = transform.position + (-transform.right * movementSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            transform.position = transform.position + (transform.right * movementSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            transform.position = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            transform.position = transform.position + (-transform.forward * movementSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.Space))
            transform.position = transform.position + (transform.up * movementSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftControl))
            transform.position = transform.position + (-transform.up * movementSpeed * Time.deltaTime);


        float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
        float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
        transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);


        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            var zoomSensitivity = fastMode ? FreeCam.fastZoomSensitivity : FreeCam.zoomSensitivity;
            transform.position = transform.position + transform.forward * axis * zoomSensitivity;
        }
    }

    #endregion


    public void EnableFreeCam()
    {
        Camera.SetupCurrent(cam);
        active = true;

    }

    public void DisableFreeCam()
    {
        active = false;
    }
}
