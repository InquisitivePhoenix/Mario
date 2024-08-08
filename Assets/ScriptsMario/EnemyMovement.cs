using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    public float upForce = 1f;
    public float destroyDelay = 2f;
    public GameObject scoreText;

    Rigidbody2D rb;
    Animator animator;

    int dir = -1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.velocity = Vector2.right * speed * dir;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * dir, rb.velocity.y);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("ground") && !collision.gameObject.CompareTag("Player"))
        {
            dir *= -1;
            rb.velocity = Vector2.right * speed * dir;
        }

        if (collision.gameObject.CompareTag("Player") && collision.otherCollider.CompareTag("enemyWeakness"))
        {
            if (scoreText != null)
            {
                scoreText.GetComponent<TextMesh>().text = "100";
                Destroy(Instantiate(scoreText, transform.position + new Vector3(0.5f, 1, 0), Quaternion.identity), 1f);
            }

            rb.velocity = Vector3.zero;
            animator.SetBool("destroyed", true);
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            if (GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().Play();
            }
            Destroy(gameObject, destroyDelay);
        }
        if (collision.gameObject.CompareTag("fire"))
        {
            rb.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
            rb.gravityScale = 4;
            animator.enabled = false;
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
            foreach (Collider2D collider in gameObject.GetComponentsInChildren<Collider2D>())
            {
                collider.isTrigger = true;
            }
        }
    }
}
