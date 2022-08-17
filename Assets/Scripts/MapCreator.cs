using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    private GameObject SandBlock = null;
    private Vector3 startPos = new Vector3(0f, 0f, 0f);
    private int maxRow;
    private int maxCol;
    private int maxFloor = -5;

    public int MaxRow { get { return maxRow; } }
    public int MaxCol { get { return maxCol; } }

    private void Awake()
    {
        SandBlock = Resources.Load<GameObject>("SandBlock");
        maxRow = 20;
        maxCol = 20;
    }
    public void Create()
    {
        int currentFloor = 0;
        while(currentFloor > maxFloor)
        {
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxCol; j++)
                {
                    startPos.x = i;
                    startPos.y = currentFloor;
                    startPos.z = j;
                    Instantiate(SandBlock, startPos, Quaternion.identity);
                }
            }
            currentFloor--;
        }
    }
}