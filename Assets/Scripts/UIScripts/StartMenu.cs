using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button playBtn, settingsBtn, quitBtn;
    [SerializeField] private int sceneToLoadIndex;
    [SerializeField] private Transform settingsUI;

    private void Start()
    {
        playBtn.onClick.AddListener(() => { SceneManager.LoadScene(sceneToLoadIndex); });
        settingsBtn.onClick.AddListener(() => { Settings(); });
        quitBtn.onClick.AddListener(() => { Application.Quit(); });
    }

    private void Settings()
    {
        settingsUI.gameObject.SetActive(true);
    }
}
