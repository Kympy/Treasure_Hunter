using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public Action keyInput = null;

    public void KeyUpdate()
    {
        if(keyInput == null)
        {
            return;
        }
        if(keyInput != null)
        {
            keyInput.Invoke();
        }
    }
    public void InitForce()
    {
        Debug.Log("Input Manager Init Force : " + GetInstanceID());
    }

    public void Awake()
    {
        Debug.Log("Input Manager Awake : " + GetInstanceID());
    }

    void OnDestroy()
    {
        //keyInput = null;
        Debug.Log("Input Manager Destroyed : " + GetInstanceID());
    }

}
