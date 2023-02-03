using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button backBtn;
    
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    
    [SerializeField] private CharacterRotation charRot;

    // Start is called before the first frame update
    private void Start()
    {
        backBtn.onClick.AddListener(() => { Back(); });
        gameObject.SetActive(false);

        Resolution();
    }

    private void Back()
    {
        gameObject.SetActive(false);
    }

    // Settings
    /*public void SetMusicVolume(float volume)
    {
        player.transform.GetChild(0).GetComponent<AudioSource>().volume = volume;
    }*/
    /*public void SetSensitivity(float sensitivity)
    {
        charRot.mouseSens = sensitivity * 200f;
    }
    public void SetInvertX(bool isInverted)
    {
        charRot.invX = isInverted;
    }
    public void SetInvertY(bool isInverted)
    {
        charRot.invY = isInverted;
    }*/
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    
    private void Resolution()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height &&
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
}
