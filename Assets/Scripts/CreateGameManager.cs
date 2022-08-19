using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGameManager : MonoBehaviour
{
    private void Awake()
    {
        GameObject obj = new GameObject("GameManager", typeof(GameManager));
    }
}
