using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button resumeBtn, settingsBtn, menuBtn;
    [SerializeField] private int startMenuSceneBuildIndex;

    [SerializeField] private Transform pauseMenu;
    private bool inMenu;

    private void Start()
    {
        resumeBtn.onClick.AddListener(() => { Hide(); });
        settingsBtn.onClick.AddListener(() => { Settings(); });
        menuBtn.onClick.AddListener(() => { SceneManager.LoadScene(startMenuSceneBuildIndex); });

        Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleMenu();
    }

    private void ToggleMenu()
    {
        if (!inMenu)
            Show();
        else
            Hide();
    }

    private void Settings()
    {
        //enable the settings UI (if there is supposed to be any)
    }

    private void Hide()
    {
        pauseMenu.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        inMenu = false;
    }

    public void Show()
    {
        pauseMenu.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        inMenu = true;
    }

}
