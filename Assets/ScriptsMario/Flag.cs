using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    public GameObject scoreText;
    public GameObject flagTriangle;
    public AudioSource flagSound;

    public LayerMask LayerMask;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject mario = collision.gameObject;
            RaycastHit2D hit = Physics2D.Raycast(mario.transform.position, Vector2.right, 3f, LayerMask);
            Debug.DrawRay(mario.transform.position, 3f * Vector2.right, Color.blue, 1f);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == gameObject.name)
                {
                    print(hit.collider.name);
                    scoreText.GetComponent<TextMesh>().text = hit.collider.name;
                    Destroy(Instantiate(scoreText, transform.position + new Vector3(0.5f, 1, 0), Quaternion.identity), 1f);
                    GameManager.score += int.Parse(hit.collider.name);
                    mario.GetComponent<PlayerInput>().enabled = false;
                    mario.GetComponent<Animator>().SetTrigger("FlagTrigger");
                    mario.GetComponent<Rigidbody2D>().gravityScale = 0;
                    mario.GetComponent<Rigidbody2D>().velocity = Vector2.down * 4;
                    gameObject.transform.parent.gameObject.SetActive(false);
                }
            }
            if (flagSound != null)
            {
                flagSound.Play();
            }

            // Triangle drop
            if (flagTriangle != null)
            {
                flagTriangle.GetComponent<Rigidbody2D>().gravityScale = 0;
                flagTriangle.GetComponent<Rigidbody2D>().velocity = Vector2.down * 4;
            }
        }
    }
}
