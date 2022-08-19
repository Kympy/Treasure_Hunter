using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotate : MonoBehaviour
{
    private Vector3 offset = Vector3.zero;
    private void Update()
    {
        transform.rotation = Quaternion.Euler(offset);
        offset.y += 10f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(this.gameObject.tag == "Star")
            {
                GameManager.Instance.GetItem();
            }
            else
            {
                GameManager.Instance.FoundCoin(this.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
