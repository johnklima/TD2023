using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMessagesUI : MonoBehaviour
{
    public static PlayerMessagesUI Instance { get; private set; }

    private TextMeshProUGUI playerMessageText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerMessageText = GetComponentInChildren<TextMeshProUGUI>();

        SetPlayerText("hello");
        HideText();
    }

    public void SetPlayerText(string text)
    {
        playerMessageText.transform.gameObject.SetActive(true);

        playerMessageText.SetText(text);
    }

    public void HideText()
    {
        playerMessageText.transform.gameObject.SetActive(false);
    }
}
