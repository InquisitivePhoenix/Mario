using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagStopPoint : MonoBehaviour
{
    public GameObject sensor;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            if (gameObject.GetComponent<AudioSource>() != null)
            {
                gameObject.GetComponent<AudioSource>().Play();
                Camera.main.GetComponent<AudioSource>().Stop();
            }
            if (sensor != null)
            {
                sensor.GetComponent<Rigidbody2D>().velocity = Vector2.up;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            GameObject mario = collision.gameObject;
            mario.GetComponent<Animator>().SetTrigger("FlagStopPoint");
            mario.GetComponent<Rigidbody2D>().gravityScale = 5;
            mario.GetComponent<Rigidbody2D>().velocity = Vector2.right * 8;
            mario.GetComponent<PlayerController>().moveInput = 1;
        }
    }
}
