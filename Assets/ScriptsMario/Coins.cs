using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public GameObject coinSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType() == typeof(CapsuleCollider2D))
        {
            GameManager.AddCoin();
            if (coinSound != null)
            {
                Instantiate(coinSound);
            }
            Destroy(gameObject);
        }
    }
    
}
