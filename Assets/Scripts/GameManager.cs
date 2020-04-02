using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UIController uIController;
    public AIController aIController;

    public enum Team
    {
        Netural,
        Red,
        Blue,
    }

    [SerializeField]
    private GameObject buildingContainer;
    public Building[] buildingList;
    public Dictionary<Team, int> buildingCounter;

    public GameObject unitContainer;
    public Unit[] unitList;

    public Team playerControlledTeam = Team.Red;

    public bool isGamePaused = false;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        buildingList = buildingContainer.gameObject.GetComponentsInChildren<Building>();
    }

    private void Update() {
        if (!isGamePaused) {
            unitList = unitContainer.gameObject.GetComponentsInChildren<Unit>();

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
    }

    private void PauseGame() {
        if (!isGamePaused) {
            isGamePaused = true;
            aIController.isAIActive = false;

            foreach (Unit unit in unitList) {
                unit.StopUnit();
            }

            foreach (Building building in buildingList) {
                building.isStopped = true;
            }
        }
    }

    private IEnumerator GameOver() {
        PauseGame();

        if (buildingCounter.ContainsKey(playerControlledTeam)) {
            uIController.DisplayWinMessage();
        } else {
            uIController.DisplayLoseMessage();
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(ExitGame());
    }

    private IEnumerator ExitGame() {
        PauseGame();

        uIController.DisplayGameOverMessage();

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(0);
    }
}
