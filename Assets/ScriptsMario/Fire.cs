using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    Rigidbody2D rb;
    public ParticleSystem fireEmber;
    public float speed = 20f;
    public float upForce = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2 (1, -0.3f) * speed * PlayerController.dir;
        Destroy(gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0.2f, 0), Vector2.right, 0.2f);
        //Debug.DrawRay(transform.position + new Vector3(0.5f, 0.2f, 0), Vector2.right * 0.2f, Color.blue, 1f);
        //print(hit.collider.gameObject.tag);
        if (!collision.gameObject.CompareTag("ground"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("ground"))
        {
            rb.AddForce(Vector2.up * upForce);
        }
    }
    private void OnDestroy()
    {
        Instantiate(fireEmber, transform.position, Quaternion.identity);
    }
}
