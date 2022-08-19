using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject target = null;
    private bool active = false;

    private float smoothSpeed = 10f;
    private Vector3 desiredPosition;

    private Vector3 posOffset = new Vector3(0f, 8f, -8f);
    private Vector3 rotOffset = new Vector3(45f, 0f, 0f);

    private float mouseX;
    private float mouseY;
    private float zoom;

    private void Start()
    {
        /*
        GameManager.Instance.GetInput.keyInput -= MouseRotation;
        GameManager.Instance.GetInput.keyInput -= MouseZoom;

        GameManager.Instance.GetInput.keyInput += MouseRotation;
        GameManager.Instance.GetInput.keyInput += MouseZoom;
        */
    }
    private void FixedUpdate()
    {
        MouseRotation();
        MouseZoom();
        if (active)
        {
            transform.LookAt(target.transform);
        }
        else return;
    }
    private void MouseRotation()
    {
        if(Input.GetMouseButton(2) && active)
        {
            mouseX = Input.GetAxis("Mouse X") * 150f;
            mouseY = Input.GetAxis("Mouse Y") * 150f;
            transform.RotateAround(target.transform.position, Vector3.up, mouseX * Time.deltaTime);
            transform.RotateAround(target.transform.position, transform.right, -mouseY * Time.deltaTime);
        }
    }
    private void MouseZoom()
    {
        if(active && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            zoom = Input.GetAxis("Mouse ScrollWheel");
            transform.position -= (transform.position - target.transform.position).normalized * zoom * 10f;
        }
    }
    public void FindPlayer()
    {
        target = GameObject.FindGameObjectWithTag("Player").gameObject;
        if (target == null)
        {
            Debug.LogError("Camera Can't Find Player!!");
            return;
        }
        active = true;
        desiredPosition = target.transform.position + posOffset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(rotOffset);
    }
}
