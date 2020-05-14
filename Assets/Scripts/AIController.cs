using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController
{
    public Team team;
    public bool isAIActive = true;
    public float timeBetweenActions = 3f;
    public float reactionSpeed = 1f;
    public int stringAbleBuildings = 3;

    public AIController(Team inTeam)
    {
        team = inTeam;
        GameManager.instance.StartCoroutine(AICoroutine());
    }

    private IEnumerator AICoroutine() {
        while(isAIActive) {
            yield return new WaitForSeconds(reactionSpeed);

            (int currentGold, List<Building> enemyBuildings, List<Building> friendlyBuildings) = GenerateInformation();

            bool isActionMade = false;
            int random = Random.Range(0, 100);
            if (random > 50) {
                if(!Attack(friendlyBuildings, enemyBuildings, currentGold)) {
                    isActionMade = Upgrade(friendlyBuildings, enemyBuildings, currentGold);
                }
            } else {
                if (!Upgrade(friendlyBuildings, enemyBuildings, currentGold)) {
                    isActionMade = Attack(friendlyBuildings, enemyBuildings, currentGold);
                }
            }

            if (isActionMade) {
                yield return new WaitForSeconds(timeBetweenActions);
            } else {
                yield return new WaitForSeconds(reactionSpeed);
            }
        }
    }

    private (int, List<Building>, List<Building>) GenerateInformation() {
        int currentGold = team.GetGold();

        List<Building> enemyBuildings = new List<Building>();
        List<Building> friendlyBuildings = new List<Building>();

        foreach (Building building in GameManager.instance.GetBuildingsList()) {
            if (building.GetTeam().Equals(team)) {
                friendlyBuildings.Add(building);
            } else {
                enemyBuildings.Add(building);
            }
        }

        enemyBuildings.Sort(SortByTroopNumber);
        friendlyBuildings.Sort(SortByTroopNumber);

        return (currentGold, enemyBuildings, friendlyBuildings);
    }

    /* Action */
    private bool Attack(List<Building> friendlyBuildings, List<Building> enemyBuildings, int currentGold) {
        // No Buildings to attack
        if (enemyBuildings.Count < 1)
        {
            return false;
        }

        // Max clicks 
        int musterableArmy = 0;
        for(int i = 0; i < friendlyBuildings.Count && i < stringAbleBuildings; i++) { 
            musterableArmy += (int) (friendlyBuildings[i].GetArmySize() / 2);
        }

        Building target = null;
        for (int i = enemyBuildings.Count - 1; i >= 0; i--) {
            // If team has less than 200 goal attempt to target a mine
            if (currentGold < 100) {
                if (enemyBuildings[i].GetType() == typeof(Mine)) {
                    target = enemyBuildings[i];
                } else if(i == 0) {
                    target = enemyBuildings[enemyBuildings.Count - 1];
                }
            } else {
                if (enemyBuildings[i].GetType() == typeof(Castle)) {
                    target = enemyBuildings[i];
                } else if (i == 0) {
                    target = enemyBuildings[enemyBuildings.Count - 1];
                }
            }
        }

        bool isAttackSent = false;
        float winPercentageDifference = 1.2f;
        int targetArmySize = (int) (target.GetArmySize() * winPercentageDifference);
        if (musterableArmy > targetArmySize) {
            int attackForce = 0;

            foreach (Building b in friendlyBuildings) {
                attackForce += (int)(b.GetArmySize() / 2);
                Unit.ConstructUnit(b, target);
                isAttackSent = true;
                if (attackForce > musterableArmy) {
                    break;
                }
            }
        } else if (friendlyBuildings.Count > 1) {
            // Reinforceing


            // Stack troops
            Building stackTarget = friendlyBuildings[0];
            for (int i = 1; i < friendlyBuildings.Count && i < stringAbleBuildings; i++) {
                if (friendlyBuildings[i].GetArmySize() >= friendlyBuildings[i].MaxGarrisonSize) {
                    Unit.ConstructUnit(friendlyBuildings[i], stackTarget);
                    isAttackSent = true;
                }
            }
        }

        return isAttackSent;
    }

    private bool Upgrade(List<Building> friendlyBuildings, List<Building> enemyBuildings, int currentGold)
    {
        foreach (Building building in friendlyBuildings)
        {
            int buildingUpgradeCost = building.GetBuildingLevel() * 100;

            if (currentGold >= buildingUpgradeCost)
            {
                currentGold -= buildingUpgradeCost;
                building.Upgrade();
                return true;
            }
        }

        return false;
    }

    /* Utility functions */
    public static int SortByTroopNumber(Building b1, Building b2)
    {
        return -b1.GetArmySize().CompareTo(b2.GetArmySize());
    }

    public static List<T> Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }
}
