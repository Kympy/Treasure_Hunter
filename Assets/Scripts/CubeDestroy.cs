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
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        maxFloor = GameManager.Instance.GetCreator.GetMaxFloor;
        if (maxFloor == transform.position.y)
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;
            GetComponent<MeshRenderer>().material.color = Color.gray;
        }
        IamCoin();
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
    public void OpenAndDestroy()
    {
        Instantiate(GameManager.Instance.smokeEffect, transform.position, transform.rotation);
        if(myType != (int)Type.Coin)
        {
            myType = Random.Range(0, 100);
            switch (myType)
            {
                case < 70: // Normal
                    {
                        Destroy(this.gameObject);
                        break;
                    }
                case < 100: // Rock
                    {
                        Vector3 desiredPos = transform.position;
                        desiredPos.y = transform.position.y - 2.1f;
                        Instantiate(GameManager.Instance.GetRock, desiredPos, Quaternion.identity);
                        Destroy(this.gameObject);
                        break;
                    }
            }
        }
        else
        {
            Instantiate(GameManager.Instance.GetCoin, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
