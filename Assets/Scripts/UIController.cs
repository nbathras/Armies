using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private void Awake() {
        backgroundCanvas.SetActive(false);
        loseText.SetActive(false);
        winText.SetActive(false);
        gameOverText.SetActive(false);

        goldResourceText.gameObject.SetActive(true);
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

    public void SetGoldResourceText(int goldResource)
    {
        goldResourceText.text = "Gold:\t" + goldResource.ToString();
    }
}
