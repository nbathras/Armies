using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static GameManager;

public class Unit : MonoBehaviour
{
    public Team team = Team.Netural;

    public TextMeshPro troopNumberText;
    public int currentTroopNumber;

    private Renderer modelRenderer;

    private NavMeshAgent agent;

    public Building origin;
    public Building target;

    private void Awake() {
        modelRenderer = gameObject.GetComponentInChildren<Renderer>();
        agent = GetComponent<NavMeshAgent>();

        SetTeam(team);
    }

    public int GetTroopNumber() {
        return currentTroopNumber;
    }

    public void SetTroopNumber(int troopNumber) {
        currentTroopNumber = troopNumber;
        troopNumberText.SetText(troopNumber.ToString());
    }

    private void Update() {
        if (target != null) {
            agent.SetDestination(target.gameObject.transform.position);
        }
    }

    public void SetTeam(Team inTeam) {
        team = inTeam;

        if (team == Team.Netural) {
            modelRenderer.material.color = Color.gray;
        } else {
            if (team == Team.Red) {
                modelRenderer.material.color = Color.red;
            }
            if (team == Team.Blue) {
                modelRenderer.material.color = Color.blue;
            }
        }
    }

    public static float spawnRadius = .5f;
    public static Unit ConstructUnit(Building inOrigin, Building inTarget) {
        Vector3 heading = inOrigin.transform.position - inTarget.transform.position;
        Vector3 headingScaled = (spawnRadius / heading.magnitude) * heading;
        Vector3 unitPosition = inOrigin.transform.position - headingScaled;

        Unit unitComponent = Instantiate(Blueprints.unitStaticPrefab, unitPosition, Quaternion.identity).GetComponent<Unit>();
        unitComponent.origin = inOrigin;
        unitComponent.target = inTarget;
        unitComponent.SetTeam(inOrigin.team);

        int numberOfDeployedTroops = (int) (inOrigin.GetTroopNumber() / 2);
        inOrigin.SetTroopNumber(inOrigin.GetTroopNumber() - numberOfDeployedTroops);
        unitComponent.SetTroopNumber(numberOfDeployedTroops);

        return unitComponent;
    }
}
