using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton for the GameManager
    public static GameManager instance;

    // Controllers
    public UIController uIController;

    // Containers to organize similar objects
    [SerializeField]
    private GameObject buildingContainer;
    [SerializeField]
    private GameObject unitContainer;
    [SerializeField]
    private GameObject teamContainer;

    // ToDo change structure so it is Dictionary<Team, (buidlingListForTeamX, unitListForTeamX)>
    // Gamestate structures
    private List<Building> buildingList;
    private List<Unit> unitList;
    private List<Team> teamList;
    // public Dictionary<Team.TeamName, Team> teamDictionary;
    // public Dictionary<Team.TeamName, int> buildingCountDictionary;

    // Game Parameters
    [SerializeField]
    private Team playerControlledTeam;
    private bool isGamePaused = false;

    /* Unity Functions */
    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        GenerateBuildingList();
        GenerateUnitList();     // will almost always be empty
        GenerateTeamList();     
    }

    private void Update()
    {
        if (!isGamePaused)
        {
            // GenerateBuildingList(); // unneccesary to run because buildings do not change after first initialization
            GenerateUnitList();
            // GenerateTeamList(); // unncessary to run because teams do not change after first initialization

            if (GetNumberRemainingTeams() < 2) {
                GameOver();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitGame();
            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                if (isGamePaused) {
                    PauseGame();
                } else {
                    UnPauseGame();
                }
            }
        }
    }



    /* Generation Commands */
    private void GenerateBuildingList()
    {
        if (buildingContainer == null) {
            throw new Exception("Custom Error: Building Container does not exist in the scene");
        }

        buildingList = buildingContainer.gameObject.GetComponentsInChildren<Building>().ToList();
    }

    private void GenerateUnitList()
    {
        if (unitContainer == null) {
            throw new Exception("Custom Error: Unit Container does not exist in the scene");
        }

        unitList = unitContainer.gameObject.GetComponentsInChildren<Unit>().ToList();
    }

    private void GenerateTeamList()
    {
        if (teamContainer == null)
        {
            throw new Exception("Custom Error: Team Container does not exist in the scene");
        }

        teamList = teamContainer.gameObject.GetComponentsInChildren<Team>().ToList();
    }


    /* Getters */
    public int GetNumberRemainingTeams() {
        int numberRemainingTeams = 0;
        foreach (Team team in teamList) {
            if (team.IsNeutral()) {
                continue;
            }

            if (team.HasBuildings()) {
                numberRemainingTeams += 1;
            }
        }

        return numberRemainingTeams;
    }

    public List<Building> GetBuildingsList()
    {
        return buildingList;
    }

    public float GetTroopPercentage()
    {
        return uIController.GetTroopPercentage();
    }

    public bool IsGamePaused() {
        return isGamePaused;
    }

    public Transform GetUnitContainer() {
        return unitContainer.transform;
    }

    public Team GetPlayerControlledTeam() {
        return playerControlledTeam;
    }

    /* Game Actions */
    private void PauseGame() {
        if (!isGamePaused) {
            isGamePaused = true;

            foreach (Unit unit in unitList) {
                unit.Pause();
            }

            foreach (Building building in buildingList) {
                building.Pause();
            }
        }
    }

    private void UnPauseGame() {
        if (isGamePaused) {
            isGamePaused = false;

            foreach (Unit unit in unitList) {
                unit.UnPause();
            }

            foreach(Building building in buildingList) {
                building.UnPause();
            }
        }
    }

    private void GameOver() {
        PauseGame();

        if (playerControlledTeam.HasBuildings()) {
            uIController.DisplayWinMessage();
        } else {
            uIController.DisplayLoseMessage();
        }

        StartCoroutine(GameOverCoroutine());

        IEnumerator GameOverCoroutine() {
            yield return new WaitForSeconds(2f);

            ExitGame();
        }
    }

    private void ExitGame() {
        PauseGame();

        uIController.DisplayGameOverMessage();

        StartCoroutine(ExitGameCoroutine());

        IEnumerator ExitGameCoroutine() {
            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene(0);
        }
    }
}
