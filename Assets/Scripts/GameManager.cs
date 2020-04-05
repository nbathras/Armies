using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UIController uIController;
    // public AIController aIController;

    [SerializeField]
    private GameObject buildingContainer;
    public Building[] buildingList;
    public Dictionary<Team.TeamOption, int> buildingCounter;

    public GameObject unitContainer;
    public Unit[] unitList;

    public Team.TeamOption playerControlledTeam = Team.TeamOption.Red;
    private List<Team> teamList;

    public bool isGamePaused = false;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        buildingList = buildingContainer.gameObject.GetComponentsInChildren<Building>();
        GenerateBuildingCounter();

        teamList = new List<Team>();
        foreach (Team.TeamOption teamOption in buildingCounter.Keys)
        {
            teamList.Add(new Team(teamOption, teamOption == playerControlledTeam));
        }
    }

    private void GenerateUnitList()
    {
        unitList = unitContainer.gameObject.GetComponentsInChildren<Unit>();
    }

    private void GenerateBuildingCounter()
    {
        buildingCounter = new Dictionary<Team.TeamOption, int>();

        foreach (Building building in buildingList)
        {
            if (building.GetTeam() == Team.TeamOption.Netural)
            {
                continue;
            }

            if (!buildingCounter.ContainsKey(building.GetTeam()))
            {
                buildingCounter[building.GetTeam()] = 0;
            }
            buildingCounter[building.GetTeam()] += 1;
        }
    }

    private void Update() {
        if (!isGamePaused) {
            GenerateUnitList();
            GenerateBuildingCounter();

            if (buildingCounter.Keys.Count < 2) {
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
            // aIController.isAIActive = false;

            foreach (Unit unit in unitList) {
                unit.StopUnit();
            }

            foreach (Building building in buildingList) {
                building.Pause();
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
