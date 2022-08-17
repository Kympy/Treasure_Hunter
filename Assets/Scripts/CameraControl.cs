using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject target = null;
    private bool active = false;

    private float smoothSpeed = 10f;
    private Vector3 desiredPosition;
    private Quaternion desiredRotation;
    private Vector3 posOffset = new Vector3(0f, 8f, -8f);
    private Vector3 rotOffset = new Vector3(45f, 0f, 0f);

    private void Update()
    {
        if (active)
        {
            desiredPosition = target.transform.position + posOffset;
            //desiredRotation = Quaternion.LookRotation(target.transform.position - transform.position);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotOffset);
        }
        else return;
    }
    public void FindPlayer()
    {
        target = GameObject.FindGameObjectWithTag("Player").gameObject;
        active = true;
        if(target == null)
        {
            Debug.LogError("Camera Can't Find Player!!");
        }
    }
}
