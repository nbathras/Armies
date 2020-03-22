using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameManager.Team team = GameManager.Team.Blue;
    public bool isAIActive = true;
    public float aIDecisionSpeed = 1f;

    void Start()
    {
        StartCoroutine(AICoroutine());
    }

    private IEnumerator AICoroutine() {
        while(isAIActive) {
            yield return new WaitForSeconds(aIDecisionSpeed);

            List<Building> enemyBuildings = new List<Building>();
            List<Building> friendlyBuildings = new List<Building>();

            foreach (Building building in GameManager.instance.buildingList) {
                if (building.team == team) {
                    friendlyBuildings.Add(building);
                } else {
                    enemyBuildings.Add(building);
                }
            }

            enemyBuildings.Sort(SortByTroopNumber);
            friendlyBuildings.Sort(SortByTroopNumber);

            if (enemyBuildings.Count > 0 && friendlyBuildings.Count > 0) {
                int choice = Random.Range(0, 4);

                if (choice == 0) {
                    if (!AttemptSingleAttack(friendlyBuildings, enemyBuildings)) {
                        AttemptUpgrade(friendlyBuildings);
                    }
                } else if (choice == 1) {
                    if (!AttemptJointAttack(friendlyBuildings, enemyBuildings)) {
                        AttemptUpgrade(friendlyBuildings);
                    }
                } else {
                    // Debug.Log("AI Attempts Upgrade first");
                    if (!AttemptUpgrade(friendlyBuildings)) {
                        AttemptSingleAttack(friendlyBuildings, enemyBuildings);
                    }
                }
            }
        }
    }

    private bool AttemptJointAttack(List<Building> friendlyBuildings, List<Building> enemyBuildings) {
        int weakestEnemy = enemyBuildings[enemyBuildings.Count - 1].GetTroopNumber();

        int attackForceSize = 0;
        List<Building> attackForce = new List<Building>();
        foreach (Building building in friendlyBuildings) {
            attackForceSize += building.GetTroopNumber() / 2;
            attackForce.Add(building);
            if (attackForceSize > weakestEnemy + 5) {
                break;
            }
        }

        if (attackForceSize > weakestEnemy + 5) {
            foreach (Building building in attackForce) {
                Unit.ConstructUnit(building, enemyBuildings[enemyBuildings.Count - 1]);
            }

            return true;
        }

        return false;
    }

    private bool AttemptSingleAttack(List<Building> friendlyBuildings, List<Building> enemyBuildings) {
        int attackForce = friendlyBuildings[0].GetTroopNumber() / 2;
        int weakestEnemy = enemyBuildings[enemyBuildings.Count - 1].GetTroopNumber();

        if (attackForce > weakestEnemy + 5) {
            Unit.ConstructUnit(friendlyBuildings[0], enemyBuildings[enemyBuildings.Count - 1]);

            return true;
        }

        return false;
    }

    private bool AttemptUpgrade(List<Building> friendlyBuildings) {
        int currentTroops = friendlyBuildings[0].GetTroopNumber();
        int maxNumberOfTroops = friendlyBuildings[0].GetMaxTroops();

        Debug.Log(currentTroops + " | " + maxNumberOfTroops);

        if (currentTroops > maxNumberOfTroops / 2 + 5) {
            friendlyBuildings[0].Upgrade();

            return true;
        }

        return false;
    }

    static int SortByTroopNumber(Building b1, Building b2) {
        return -b1.GetTroopNumber().CompareTo(b2.GetTroopNumber());
    }
}
