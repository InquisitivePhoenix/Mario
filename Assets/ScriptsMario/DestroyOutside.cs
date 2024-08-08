using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOutside : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3);
        GameManager.LoadScreenOn();
        yield return new WaitForSeconds(3);

        GameManager.LoadScreenOff();
        if (GameManager.health > 0)
        {
            GameManager.RestartLevel();
            GameManager.health--;
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
            SceneManager.LoadScene("1-1", LoadSceneMode.Additive);
        }
    }
}
