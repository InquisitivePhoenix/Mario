using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public Sprite newSprite;
    public GameObject item;
    public LayerMask LayerMask;
    public ParticleSystem bricksBreak;
    public int hp = 1;

    private bool isActive = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RaycastHit2D hit = Physics2D.Raycast(collision.gameObject.transform.position + Vector3.up * 0.5f, Vector2.up, 3f, LayerMask);
        Debug.DrawLine(collision.gameObject.transform.position + Vector3.up * 0.5f, collision.gameObject.transform.position + Vector3.up, Color.blue, 1f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.name == gameObject.name && collision.gameObject.CompareTag("Player"))
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                Hit();
            }
        }
    }

    private void Hit()
    {
        if (isActive)
        {
            hp--;
            StartCoroutine(Animate());
            ChangeSprite();
            DropItem();
            if (PlayerController.grown && hp <= 0 && newSprite == null && bricksBreak != null)
            {
                Instantiate(bricksBreak, transform.position, Quaternion.identity);
                Destroy(gameObject, 0.05f);
            }
        }
    }

    void DropItem()
    {
        if (item != null)
        {
            Instantiate(item, (Vector2)transform.localPosition, Quaternion.identity);
        }
    }

    private IEnumerator Animate()
    {
        Vector2 restingPos = transform.localPosition;
        Vector2 animatedPos = (Vector2)transform.localPosition + 0.5f * Vector2.up;

        yield return Move(restingPos, animatedPos);
        yield return Move(animatedPos, restingPos);
    }

    private IEnumerator Move(Vector2 startPoint, Vector2 endPoint)
    {
        float currentTime = 0f;
        float duration = 0.1f;

        while (currentTime < duration)
        {
            float t = currentTime / duration;
            transform.localPosition = Vector2.Lerp(startPoint, endPoint, t);

            currentTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = endPoint;
    }

    void ChangeSprite()
    {
        if (newSprite != null && hp == 0)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = newSprite;
            isActive = false;
        }
    }
}
