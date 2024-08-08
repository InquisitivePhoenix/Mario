using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    VisualElement mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        if (!SceneManager.GetSceneByName("1-1").isLoaded)
        {
            SceneManager.LoadSceneAsync("1-1", LoadSceneMode.Additive);
        }
        Time.timeScale = 0;
        UIDocument document = GetComponent<UIDocument>();
        mainMenu = document.GetComponent<UIDocument>().rootVisualElement;
        mainMenu.Q<Button>("start-btn").clicked += () => StartGame();
        mainMenu.Q<Button>("quit-btn").clicked += () => QuitGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartGame()
    {
        Time.timeScale = 1;
        mainMenu.Q<VisualElement>("MainMenu").visible = false;
        mainMenu.Q<VisualElement>("MainMenu").SetEnabled(false);
        mainMenu.Q<VisualElement>("MainMenu").style.display = DisplayStyle.Flex;
        Camera.main.GetComponent<AudioSource>().Play();
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("MainMenu"));
    }

    void QuitGame()
    {
        Application.Quit();
    }

}
