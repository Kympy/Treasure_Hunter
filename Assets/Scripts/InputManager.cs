using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : SingleTon<InputManager>
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
}
