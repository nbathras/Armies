using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController
{
    // public GameManager.Team team = GameManager.Team.Blue;
    public Team.TeamName teamName;
    public bool isAIActive = true;
    public float aIDecisionSpeed = 1f;

    public AIController(Team.TeamName inTeamName)
    {
        teamName = inTeamName;
        GameManager.instance.StartCoroutine(AICoroutine());
    }

    private IEnumerator AICoroutine() {
        while(isAIActive) {
            yield return new WaitForSeconds(aIDecisionSpeed);

            int currentGold = GameManager.instance.GetTeam(teamName).GetGold();

            List<Building> enemyBuildings = new List<Building>();
            List<Building> friendlyBuildings = new List<Building>();
            Dictionary<Team.TeamName, int> troopCount = new Dictionary<Team.TeamName, int>();

            foreach (Building building in GameManager.instance.GetBuildingsList()) {
                if (building.GetTeamName() == teamName) {
                    friendlyBuildings.Add(building);
                } else {
                    enemyBuildings.Add(building);
                }

                if (!troopCount.ContainsKey(building.GetTeamName()))
                {
                    troopCount[building.GetTeamName()] = 0;
                }
                troopCount[building.GetTeamName()] += building.GetArmySize();
            }

            enemyBuildings.Sort(SortByTroopNumber);
            friendlyBuildings = Shuffle(friendlyBuildings);

            if (currentGold < 300)
            {
                Attack(friendlyBuildings, enemyBuildings, typeof(Mine));
            } else {
                Attack(friendlyBuildings, enemyBuildings, typeof(Building));
            }

            Upgrade(friendlyBuildings);


            /*
            if (enemyBuildings.Count > 0 && friendlyBuildings.Count > 0) {
                int choice = Random.Range(0, 2);

                if (choice == 0) {
                    AttemptJointAttack(friendlyBuildings, enemyBuildings);
                } else {
                    AttemptJointUpgrade(friendlyBuildings);
                }
            }
            */
        }
    }

    private void Attack(List<Building> friendlyBuildings, List<Building> enemyBuildings, System.Type targetPreference) {
        if (enemyBuildings.Count < 1)
        {
            return;
        }

        Building target = null;
        for (int i = enemyBuildings.Count - 1; i >= 0; i--)
        {
            if (enemyBuildings[i].GetType() == targetPreference)
            {
                target = enemyBuildings[i];
                break;
            }
        }

        if (target == null)
        {
            target = enemyBuildings[enemyBuildings.Count - 1];
        }

        int targetForce = target.GetArmySize();

        int attackForceSize = 0;
        List<Building> attackForce = new List<Building>();
        foreach (Building building in friendlyBuildings) {
            attackForceSize += building.GetArmySize() / 2;
            attackForce.Add(building);
            if (attackForceSize > targetForce + 5) {
                break;
            }
        }

        if (attackForceSize > targetForce + 5) {
            foreach (Building building in attackForce) {
                Unit.ConstructUnit(building, enemyBuildings[enemyBuildings.Count - 1]);
            }
        }
    }

    private void Upgrade(List<Building> friendlyBuildings)
    {
        int currentGold = GameManager.instance.GetTeam(teamName).GetGold();

        foreach (Building building in friendlyBuildings)
        {
            int buildingUpgradeCost = building.GetBuildingLevel() * 100;

            if (currentGold >= buildingUpgradeCost)
            {
                currentGold -= buildingUpgradeCost;
                building.AttemptUpgrade();
            }
        }
    }

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
