using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public string team = "blue";

    public GameObject buildingContainer;
    private Building[] buildingList;

    void Start()
    {
        buildingList = buildingContainer.gameObject.GetComponentsInChildren<Building>();
    }

    void Update()
    {
        List<Building> enemyBuildings = new List<Building>();
        List<Building> friendlyBuildings = new List<Building>();

        foreach (Building building in buildingList) {
            if (building.team.Equals(team)) {
                friendlyBuildings.Add(building);
            } else {
                enemyBuildings.Add(building);
            }
        }

        enemyBuildings.Sort(SortByTroopNumber);
        friendlyBuildings.Sort(SortByTroopNumber);

        Debug.Log("test1: " + friendlyBuildings[0].GetTroopNumber());
        Debug.Log("test2: " + enemyBuildings[0].GetTroopNumber());

        if (enemyBuildings.Count > 0 && friendlyBuildings.Count > 0) {
            if (((int) friendlyBuildings[0].GetTroopNumber() / 2) > enemyBuildings[enemyBuildings.Count -1].GetTroopNumber() + 10) {
                Unit.ConstructUnit(friendlyBuildings[0], enemyBuildings[enemyBuildings.Count - 1]);
            }
        }
    }

    static int SortByTroopNumber(Building b1, Building b2) {
        return -b1.GetTroopNumber().CompareTo(b2.GetTroopNumber());
    }
}
