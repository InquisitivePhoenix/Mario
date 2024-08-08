using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public const int maxHealth = 1;
    public static int health = 1;
    public static int coinsAmount = 0;
    public static int score = 0;
    public static bool isPaused = false;
    public static bool instance = false;

    private UIDocument loadScreen;
    public static VisualElement rootLoadScreen;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        coinsAmount = 0;
        score = 0;
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            loadScreen = GetComponent<UIDocument>();
            rootLoadScreen = loadScreen.rootVisualElement;
            LoadScreenOff();
            instance = true;
        }
        DontDestroyOnLoad(gameObject);
    }

    public static void AddCoin()
    {
        coinsAmount++;
        score += 200;
        Debug.Log(coinsAmount + " coins");
        if (coinsAmount > 100)
        {
            health++;
            coinsAmount = 0;
        }
    }

    public void PauseGame()
    {

        if (!isPaused)
        {
            Time.timeScale = 0;
        }
        else 
        {
            Time.timeScale = 1;
        }
        isPaused = !isPaused;

    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    } 

    public static void LoadScreenOn()
    {
        rootLoadScreen.Q<VisualElement>("LoadScreen").style.display = DisplayStyle.Flex;
        if (health > 0)
        {
            

            rootLoadScreen.Q<VisualElement>("mario-pic").style.display = DisplayStyle.Flex;
            rootLoadScreen.Q<Label>("hp-label").text = " x " + health;
        }
        else
        {
            rootLoadScreen.Q<VisualElement>("mario-pic").style.display = DisplayStyle.None;
            rootLoadScreen.Q<Label>("hp-label").text = "Game Over";
        }
        
    }
    public static void LoadScreenOff()
    {
        rootLoadScreen.Q<VisualElement>("LoadScreen").style.display = DisplayStyle.None;
        rootLoadScreen.Q<VisualElement>("mario-pic").style.display = DisplayStyle.None;
    }

    public static void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnLevelWasLoaded(int level)
    {
        if (Camera.main != null) { 
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }






}
