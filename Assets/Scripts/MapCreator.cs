using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    private GameObject SandBlock = null;
    private GameObject temp = null;
    private Vector3 startPos = new Vector3(0f, 0f, 0f); // Cube Start Position
    private int maxRow;
    private int maxCol;
    private int maxFloor = -6;
    public int GetMaxFloor { get { return maxFloor; } }

    public int MaxRow { get { return maxRow; } }
    public int MaxCol { get { return maxCol; } }

    private void Awake()
    {
        SandBlock = Resources.Load<GameObject>("SandBlock"); // Load Prefab
        maxRow = 20; // Size
        maxCol = 20;
    }
    public void Create() // Create Cube Block
    {
        GameObject Map = new GameObject("Map"); // Map Parent Empty GameObject

        int currentFloor = 0;
        while(currentFloor >= maxFloor) // To Max Floor
        {
            for (int i = 0; i < maxRow; i++) // Row Size
            {
                for (int j = 0; j < maxCol; j++) // Column Size
                {
                    startPos.x = i;
                    startPos.y = currentFloor; // Floor
                    startPos.z = j;
                    temp = Instantiate(SandBlock, startPos, Quaternion.identity); // Create Cube
                    temp.transform.parent = Map.transform; // Set Parent
                }
            }
            currentFloor--; // Floor goes down
        }
    }
}
