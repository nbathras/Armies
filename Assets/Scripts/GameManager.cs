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
    public GameObject buildingContainer;
    public GameObject unitContainer;

    // Gamestate structures
    private List<Building> buildingList;
    private Unit[] unitList;
    public Dictionary<Team.TeamName, Team> teamDictionary;
    public Dictionary<Team.TeamName, int> buildingCountDictionary;

    // Game Parameters
    public Team.TeamName playerControlledTeam = Team.TeamName.Red;
    public bool isGamePaused = false;

    /* Unity Functions */
    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        GenerateBuildingList();
        GenerateTeams();
    }

    private void Update()
    {
        if (!isGamePaused)
        {
            GenerateUnitList();
            GenerateBuildingCount();

            if (buildingCountDictionary.Keys.Count < 2)
            {
                StartCoroutine(GameOver(buildingCountDictionary.ContainsKey(playerControlledTeam)));
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(ExitGame());
            }
        }
    }



    /* Generation Commands */
    private void GenerateBuildingList()
    {
        buildingList = buildingContainer.gameObject.GetComponentsInChildren<Building>().ToList();
    }

    private void GenerateUnitList()
    {
        unitList = unitContainer.gameObject.GetComponentsInChildren<Unit>();
    }

    private void GenerateTeams()
    {
        if (buildingList == null)
        {
            throw new Exception("Custom Error: Cannot generate teams with null buidling list");
        }

        teamDictionary = new Dictionary<Team.TeamName, Team>();
        foreach (Building building in buildingList)
        {
            Team.TeamName buildingTeam = building.GetTeamName();

            if (!teamDictionary.ContainsKey(buildingTeam) && buildingTeam != Team.TeamName.Netural)
            {
                teamDictionary[buildingTeam] = new Team(
                    buildingTeam,
                    buildingTeam == playerControlledTeam
                );
            }
        }
    }

    private void GenerateBuildingCount()
    {
        buildingCountDictionary = new Dictionary<Team.TeamName, int>();
        foreach (Building building in buildingList)
        {
            Team.TeamName buildingTeam = building.GetTeamName();
            if (buildingTeam == Team.TeamName.Netural)
            {
                continue;
            }

            if (!buildingCountDictionary.ContainsKey(buildingTeam))
            {
                buildingCountDictionary[buildingTeam] = 0;
            }
            buildingCountDictionary[buildingTeam] += 1;
        }
    }



    /* Getters */
    public Team GetTeam(Team.TeamName teamOption)
    {
        return teamDictionary[teamOption];
    }

    public List<Building> GetBuildingsList()
    {
        return buildingList;
    }



    /* Game Actions */
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

    private IEnumerator GameOver(bool isWin) {
        PauseGame();

        if (isWin) {
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
