using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsFromBlock : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (Time.timeSinceLevelLoad > 1f)
        {
            GameManager.AddCoin();

            StartCoroutine(Animate());
            Destroy(gameObject, 0.6f);
        }
    }

    private IEnumerator Animate()
    {
        Vector2 restingPos = transform.localPosition;
        Vector2 animatedPos = (Vector2)transform.localPosition + 3f * Vector2.up;

        yield return Move(restingPos, animatedPos);
        yield return Move(animatedPos, restingPos);
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
    }
}
