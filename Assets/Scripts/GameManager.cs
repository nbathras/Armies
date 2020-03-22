using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum Team
    {
        Netural,
        Red,
        Blue,
    }

    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private AIController aiController;
    [SerializeField]
    private Blueprints bluePrints;

    [SerializeField]
    private GameObject backgroundCanvas;
    [SerializeField]
    private GameObject loseText;
    [SerializeField]
    private GameObject winText;
    [SerializeField]
    private GameObject gameOverText;

    [SerializeField]
    private GameObject buildingContainer;
    public Building[] buildingList;
    public Dictionary<Team, int> buildingCounter;

    public Team playerControlledTeam = Team.Red;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        buildingList = buildingContainer.gameObject.GetComponentsInChildren<Building>();

        backgroundCanvas.SetActive(false);
        loseText.SetActive(false);
        winText.SetActive(false);
        gameOverText.SetActive(false);
    }

    private void Update() {
        buildingCounter = new Dictionary<Team, int>();

        foreach (Building building in buildingList) {
            if (!buildingCounter.ContainsKey(building.team)) {
                buildingCounter[building.team] = 0;
            }
            buildingCounter[building.team] += 1;
        }

        int numberOfRemainingTeams = 0;
        foreach (Team team in buildingCounter.Keys) {
            if (team != Team.Netural && buildingCounter[team] > 0) {
                numberOfRemainingTeams += 1;
            }
        }

        if (numberOfRemainingTeams < 2) {
            StartCoroutine(GameOver());
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            StartCoroutine(ExitGame());
        }
    }

    private IEnumerator GameOver() {
        backgroundCanvas.SetActive(true);
        if (buildingCounter.ContainsKey(playerControlledTeam)) {
            winText.SetActive(true);
        } else {
            loseText.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(ExitGame());
    }

    private IEnumerator ExitGame() {
        backgroundCanvas.SetActive(true);
        gameOverText.SetActive(true);
        loseText.SetActive(false);
        winText.SetActive(false);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(0);
    }
}
