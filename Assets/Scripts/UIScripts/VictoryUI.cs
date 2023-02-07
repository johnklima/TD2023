using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuBtn, quitBtn;

    private void Start()
    {
        mainMenuBtn.onClick.AddListener(() => { MainMenu(); });
        quitBtn.onClick.AddListener(() => { Application.Quit(); });
    }

    private void MainMenu()
    {
        //go to menu
    }
}
