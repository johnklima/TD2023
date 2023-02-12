using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuBtn, quitBtn;

    private void Start()
    {
        mainMenuBtn.onClick.AddListener(() => { MainMenu(); });
        quitBtn.onClick.AddListener(() => { Application.Quit(); });

        gameObject.SetActive(false);
    }

    private void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    //listener above not working?
    public void Quit()
    {
        Application.Quit();
    }
}
