using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroy : MonoBehaviour
{

    private enum Type
    {
        None,
        Rock,
        Coin,
    }

    private int myType;
    private Rigidbody rigid;
    private int maxFloor;
    private void Awake() // When Cube Created
    {
        rigid = GetComponent<Rigidbody>(); // Get
        maxFloor = GameManager.Instance.GetCreator.GetMaxFloor; // Max floor
        if (maxFloor == transform.position.y) // If I am max floor,
        {
            rigid.useGravity = false; // My gravity scale is 0
            rigid.isKinematic = true; // I am kinematic object
            GetComponent<MeshRenderer>().material.color = Color.gray; // My Color is Gray
        }
        IamCoin(); // Am I Coin Block?
    }
    private void IamCoin()
    {
        int i = Random.Range(0, 100);
        if(i < 1 && maxFloor != transform.position.y)
        {
            myType = (int)Type.Coin;
            GameManager.Instance.TotalCoinCount++;
            GameManager.Instance.GetCoinList.Add(this.gameObject);
        }
    }
    public void OpenAndDestroy() // Set my type And Destroy and Create
    {
        Instantiate(GameManager.Instance.smokeEffect, transform.position, transform.rotation); // Show Smoke Effect
        if(myType != (int)Type.Coin) // If I am not a coin block,
        {
            myType = Random.Range(0, 100); // Random set
            switch (myType)
            {
                case < 10: // Star Item
                    {
                        Instantiate(GameManager.Instance.GetStar, transform.position, Quaternion.identity); // Create Item
                        break;
                    }
                case < 90: // Normal block
                    {
                        break; // Do Nothing
                    }
                case < 100: // Rock Block
                    {
                        Vector3 desiredPos = transform.position;
                        desiredPos.y = transform.position.y - 2.1f; // Need to set rock block's position, because of the prefab pos
                        Instantiate(GameManager.Instance.GetRock, desiredPos, Quaternion.identity); // Create Rock block
                        break;
                    }
            }
            Destroy(this.gameObject); // Destroy original sand block
        }
        else // If I am a coin block,
        {
            Instantiate(GameManager.Instance.GetCoin, transform.position, Quaternion.identity); // Create coin
            Destroy(this.gameObject); // and destroy me.
        }
    }
}
