using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : class, new()
{
    private static volatile T instance;
    private static object lockObj = new object();

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(T)) as T;
                lock(lockObj)
                {
                    if (instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).ToString(), typeof(T));
                        instance = obj.GetComponent<T>();
                    }
                }
                return instance;
            }
            return instance;
        }
    }
    public virtual void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }
}
