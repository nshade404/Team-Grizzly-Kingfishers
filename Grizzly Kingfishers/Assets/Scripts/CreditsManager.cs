using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CreditsManager : MonoBehaviour
{
    public Text creditsText;
    public string creditsFilePath = "Assets/RocketAwayCredits.txt";

    private string[] creditLines;
    private float startYPos;
    private float currentYPos;

    void Start()
    {
        LoadCredits();
    }

    void LoadCredits()
    {
        if (File.Exists(creditsFilePath))
        {
            creditLines = File.ReadAllLines(creditsFilePath);
            string creditsContent = string.Join("\n", creditLines);
            creditsText.text = creditsContent;

            RectTransform rectTransform = creditsText.rectTransform;
            startYPos = rectTransform.rect.height / 2f;
            currentYPos = startYPos;
        }
        else
        {
            Debug.LogError("Credits file not found at path: " + creditsFilePath);
        }
    }
}
