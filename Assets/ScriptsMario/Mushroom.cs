using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Mushroom : MonoBehaviour
{
    public float speed = 1f;
    Rigidbody2D rb;
    int dir = 1;
    bool flower = false;
    public Sprite flowerSprite;
    public static SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        if (PlayerController.grown)
        {
            spriteRenderer.sprite = flowerSprite;
            animator.SetBool("flower", true);
        }
        StartCoroutine(Animate());
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = Vector2.right * speed * dir;
    }

    private void Update()
    {
        
    }

    private IEnumerator Animate()
    {
        Vector2 restingPos = transform.localPosition;
        Vector2 animatedPos = (Vector2)transform.localPosition + Vector2.up;
        yield return Move(restingPos, animatedPos);
    }

    private IEnumerator Move(Vector2 startPoint, Vector2 endPoint)
    {
        float currentTime = 0f;
        float duration = 0.4f;

        while (currentTime < duration)
        {
            float t = currentTime / duration;
            transform.localPosition = Vector2.Lerp(startPoint, endPoint, t);

            currentTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = endPoint;
        rb.simulated = true;
        if (PlayerController.grown)
        {
            speed = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("ground") && !collision.gameObject.CompareTag("Player"))
        {
            dir *= -1;
        }
    }
    
}
