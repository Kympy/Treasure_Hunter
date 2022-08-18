using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    private GameObject SandBlock = null;
    private GameObject temp = null;
    private Vector3 startPos = new Vector3(0f, 0f, 0f);
    private int maxRow;
    private int maxCol;
    private int maxFloor = -6;
    public int GetMaxFloor { get { return maxFloor; } }

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
        GameObject Map = new GameObject("Map");

        int currentFloor = 0;
        while(currentFloor >= maxFloor)
        {
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxCol; j++)
                {
                    startPos.x = i;
                    startPos.y = currentFloor;
                    startPos.z = j;
                    temp = Instantiate(SandBlock, startPos, Quaternion.identity);
                    temp.transform.parent = Map.transform;
                }
            }
            currentFloor--;
        }
    }
}
