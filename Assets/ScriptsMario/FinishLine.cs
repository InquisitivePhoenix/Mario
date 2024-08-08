using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public ParticleSystem firework;
    public Transform fireworkTransform;
    public GameObject flagVictory;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        StartCoroutine(Fireworks());
        StartCoroutine(Animate(flagVictory));
        StartCoroutine(NextLevel());
    }

    IEnumerator Fireworks()
    {
        Instantiate(firework, fireworkTransform.position, Quaternion.identity);
        yield return new WaitForSeconds(5);
        
    }

    private IEnumerator Animate(GameObject gameObject)
    {
        Vector2 restingPos = gameObject.transform.localPosition;
        Vector2 animatedPos = (Vector2)gameObject.transform.localPosition + Vector2.right;

        yield return Move(restingPos, animatedPos, flagVictory);
    }

    private IEnumerator Move(Vector2 startPoint, Vector2 endPoint, GameObject gameObject)
    {
        float currentTime = 0f;
        float duration = 0.7f;

        while (currentTime < duration)
        {
            float t = currentTime / duration;
            gameObject.transform.localPosition = Vector2.Lerp(startPoint, endPoint, t);

            currentTime += Time.deltaTime;

            yield return null;
        }

        gameObject.transform.localPosition = endPoint;
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(5);
        GameManager.LoadScreenOn();
        yield return new WaitForSeconds(3);

        GameManager.LoadScreenOff();
        SceneManager.LoadScene("MainMenu");
        SceneManager.LoadScene("1-1", LoadSceneMode.Additive);
    }
}
