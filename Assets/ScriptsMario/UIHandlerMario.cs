using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIHandlerMario : MonoBehaviour
{
    VisualElement score;
    VisualElement coinAmount;
    VisualElement levelNumber;
    VisualElement time;

    VisualElement mainMenu;
    float currentTime = 300;
    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0;
        UIDocument uiDocument = GetComponent<UIDocument>();


        score = uiDocument.rootVisualElement.Q<Label>("Score");
        coinAmount = uiDocument.rootVisualElement.Q<Label>("CoinAmount");
        levelNumber = uiDocument.rootVisualElement.Q<Label>("LevelNumber");
        time = uiDocument.rootVisualElement.Q<Label>("Seconds");

        uiDocument.rootVisualElement.Q<Button>("mainMenu-btn").clicked += () => BackToMenu();
        uiDocument.rootVisualElement.Q<Button>("quit-btn").clicked += () => QuitGame();

    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        currentTime = currentTime <= 0 ? 0 : currentTime;

        score.Q<Label>().text = GameManager.score.ToString();
        coinAmount.Q<Label>().text = " x " + GameManager.coinsAmount;
        levelNumber.Q<Label>().text = SceneManager.GetActiveScene().name;
        time.Q<Label>().text = " " + (Mathf.CeilToInt(currentTime));
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        //SceneManager.LoadScene("1-1", LoadSceneMode.Additive);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
