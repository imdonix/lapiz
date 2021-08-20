using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FreeCam : MonoBehaviour
{

    private float movementSpeed = 20f;
    private float fastMovementSpeed = 50f;
    private float freeLookSensitivity = 3f;
    private float zoomSensitivity = 10f;
    private float fastZoomSensitivity = 50f;
    private bool looking = false;
    private bool active;

    private Camera cam;

    private void Awake()
    {
        this.cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!active) return;

        bool fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float movementSpeed = fastMode ? this.fastMovementSpeed : this.movementSpeed;

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

        if (looking)
        {
            float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
            float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
            transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }

        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            var zoomSensitivity = fastMode ? this.fastZoomSensitivity : this.zoomSensitivity;
            transform.position = transform.position + transform.forward * axis * zoomSensitivity;
        }
    }

    private void StartLooking()
    {
        looking = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void StopLooking()
    {
        looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void EnableFreeCam()
    {
        Camera.SetupCurrent(cam);
        StartLooking();
        active = true;

    }

    public void DisableFreeCam()
    {
        StopLooking();
        active = false;
    }
}
