using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaShell : MonoBehaviour
{
    public float speed = 2f;
    public float upForce = 1f;
    public float destroyDelay = 2f;

    private bool isDangerous = false;

    Rigidbody2D rb;
    int dir = -1;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.AddForce(Vector2.up * upForce * 0.5f, ForceMode2D.Impulse);
        //rb.velocity = Vector2.right * speed * dir;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!collision.gameObject.CompareTag("ground") && !collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("enemy"))
        {
            dir *= -1;
            rb.velocity = 5 * dir * speed * Vector2.right;
        }

        if (isDangerous)
        {
            gameObject.tag = "enemy";
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            dir = -(int)Mathf.Sign((collision.transform.position.x - gameObject.transform.position.x)); // Панцирь должен лететь в сторону от Марио
            rb.velocity = 7 * dir * speed * Vector2.right;
            isDangerous = true;
        }

        if (collision.gameObject.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            Destroy(collision.gameObject, 1);  
        }
    }
}
