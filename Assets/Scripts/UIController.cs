using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject backgroundCanvas;
    [SerializeField]
    private GameObject loseText;
    [SerializeField]
    private GameObject winText;
    [SerializeField]
    private GameObject gameOverText;

    [SerializeField]
    private TextMeshProUGUI goldResourceText;

    [SerializeField]
    private Button troopButton25;
    [SerializeField]
    private Button troopButton50;
    [SerializeField]
    private Button troopButton75;

    [SerializeField]
    private Sprite onSprite25;
    [SerializeField]
    private Sprite onSprite50;
    [SerializeField]
    private Sprite onSprite75;
    [SerializeField]
    private Sprite offSprite25;
    [SerializeField]
    private Sprite offSprite50;
    [SerializeField]
    private Sprite offSprite75;

    [SerializeField]
    private Button arrowButton;

    [SerializeField]
    private Sprite onSpriteArrows;
    [SerializeField]
    private Sprite offSpriteArrows;

    private float troopSizePercent;

    private void Awake() {
        backgroundCanvas.SetActive(false);
        loseText.SetActive(false);
        winText.SetActive(false);
        gameOverText.SetActive(false);

        goldResourceText.gameObject.SetActive(true);

        SetTroopPercentage(.5f);
    }

    public void DisplayLoseMessage() {
        backgroundCanvas.SetActive(true);
        loseText.SetActive(true);
        winText.SetActive(false);
        gameOverText.SetActive(false);
    }

    public void DisplayWinMessage() {
        backgroundCanvas.SetActive(true);
        loseText.SetActive(false);
        winText.SetActive(true);
        gameOverText.SetActive(false);
    }

    public void DisplayGameOverMessage() {
        backgroundCanvas.SetActive(true);
        loseText.SetActive(false);
        winText.SetActive(false);
        gameOverText.SetActive(true);
    }

    public float GetTroopPercentage()
    {
        return troopSizePercent;
    }

    public void SetGoldResourceText(int goldResource)
    {
        goldResourceText.text = "Gold:\t" + goldResource.ToString();
    }

    public void SetTroopPercentage(float percentage)
    {
        troopSizePercent = percentage;

        if (percentage == .25f)
        {
            troopButton25.GetComponent<Image>().sprite = onSprite25;
            troopButton50.GetComponent<Image>().sprite = offSprite50;
            troopButton75.GetComponent<Image>().sprite = offSprite75;
        } else if (percentage == .50f)
        {
            troopButton25.GetComponent<Image>().sprite = offSprite25;
            troopButton50.GetComponent<Image>().sprite = onSprite50;
            troopButton75.GetComponent<Image>().sprite = offSprite75;
        } else if (percentage == .75f)
        {
            troopButton25.GetComponent<Image>().sprite = offSprite25;
            troopButton50.GetComponent<Image>().sprite = offSprite50;
            troopButton75.GetComponent<Image>().sprite = onSprite75;
        }
    }

    public void toggleArrow(Boolean nextState)
    {
        arrowButton.GetComponent<Image>().sprite = nextState ? onSpriteArrows : offSpriteArrows;
    }
}
