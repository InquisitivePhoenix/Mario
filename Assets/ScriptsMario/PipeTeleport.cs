using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PipeTeleport : MonoBehaviour
{
    public GameObject virtualCamera;
    public bool on;
    public float pipeDirectionX;
    public float pipeDirectionY;

    public Transform pipeExitPosition;

    private bool isFall = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isFall)
        {
            if (collision.gameObject.GetComponent<PlayerController>().moveInput == pipeDirectionX &&
                collision.gameObject.GetComponent<PlayerController>().moveInputY == pipeDirectionY)
            {
                Rigidbody2D marioRb = collision.gameObject.GetComponent<Rigidbody2D>();
                StartCoroutine(MoveThroughPipe(marioRb));
                isFall = true;
            }
        }
    }

    IEnumerator MoveThroughPipe(Rigidbody2D rb)
    {
        // Звуки трубы
        GetComponent<AudioSource>().Play();

        // Марио отключает коллайдеры и проваливается в трубу, потом главная камера выключается, вторая виртуальная камера включается,
        // после задержки Марио переносится под землю, останавливается, включаются гравитация, главная камера и коллайдеры.

        foreach (Collider2D collider in rb.gameObject.GetComponents<Collider2D>())
        {
            collider.isTrigger = true;
        }

        rb.gameObject.GetComponent<PlayerInput>().enabled = false;
        rb.gravityScale = 0;
        rb.velocity = -(rb.position - (Vector2)gameObject.transform.position).normalized * 4; // Fix it
        yield return new WaitForSeconds(1f);

        Camera cam = Camera.main;
        cam.enabled = false;
        virtualCamera.SetActive(on);
        rb.MovePosition(pipeExitPosition.position);
        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;
        rb.gravityScale = 5;
        cam.enabled = true;
        yield return new WaitForSeconds(0.1f);

        foreach (Collider2D collider in rb.gameObject.GetComponents<Collider2D>())
        {
            collider.isTrigger = false;
        }
        rb.gameObject.GetComponent<PlayerInput>().enabled = true;
    }
}
