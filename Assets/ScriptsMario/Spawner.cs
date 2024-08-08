using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemies.Count != 0 && collision.CompareTag("Player"))
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                enemy.SetActive(true);
                }
            }
        }
    }
}
