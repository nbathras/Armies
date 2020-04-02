using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static GameManager;

public class Building : MonoBehaviour
{
    private const int STARTING_MAX_TROOP = 100;
    private const int STARTING_TROOP_GENERATION_RATE = 10;

    public int currentLevel = 1;
    public bool isStopped = false;

    public GameObject[] buildingModelStages;

    public Team team = Team.Netural;
    public TextMeshPro troopNumberText;
    public int currentTroopNumber;
    public GameObject selectionCircle;

    private Renderer modelRenderer;

    private void Awake() {
        selectionCircle.SetActive(false);
        SetTroopNumber(currentTroopNumber);
        SetLevel(currentLevel);
        isStopped = false;

        StartCoroutine(GenerateUnits());
    }

    private void SetLevel(int level) {
        currentLevel = level;

        for (int i = 0; i < buildingModelStages.Length; i++) {
            if (i == currentLevel - 1) {
                buildingModelStages[i].SetActive(true);
            } else {
                buildingModelStages[i].SetActive(false);
            }
        }

        SetTeam(team);
    }

    private IEnumerator GenerateUnits() {
        while(true) {
            yield return new WaitForSeconds(1f);

            if (team != Team.Netural&& !isStopped) {
                int troopNumber = currentTroopNumber + GetTroopGenerationRate();

                if (troopNumber > GetMaxTroops()) {
                    troopNumber = GetMaxTroops();
                }

                SetTroopNumber(troopNumber);
            }
        }
    }

    public int GetTroopNumber() {
        return currentTroopNumber;
    }

    public void SetTroopNumber(int troopNumber) {
        currentTroopNumber = troopNumber;
        troopNumberText.SetText(currentTroopNumber.ToString());
    }

    public int GetMaxTroops() {
        return STARTING_MAX_TROOP * currentLevel;
    }

    public int GetTroopGenerationRate() {
        return STARTING_TROOP_GENERATION_RATE * currentLevel;
    }

    public bool Upgrade() {
        int currentTroopNumber = GetTroopNumber();

        if (currentTroopNumber >= GetMaxTroops() / 2 && currentLevel < buildingModelStages.Length) {
            SetTroopNumber(currentTroopNumber - GetMaxTroops() / 2);

            SetLevel(currentLevel + 1);

            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Unit") {
            Unit unitObject = collision.gameObject.GetComponent<Unit>();

            if (unitObject.origin != this && unitObject.target == this) {
                int newTroopValue;
                if (unitObject.team.Equals(team)) {
                    newTroopValue = unitObject.GetTroopNumber() + GetTroopNumber();
                } else {
                    newTroopValue = GetTroopNumber() - unitObject.GetTroopNumber();
                    if (newTroopValue < 0) {
                        SetTeam(unitObject.team);
                        newTroopValue *= -1;
                    }
                }
                SetTroopNumber(newTroopValue);
                Destroy(collision.gameObject);
            }
        }
    }

    public void SetTeam(Team inTeam) {
        team = inTeam;

        modelRenderer = buildingModelStages[currentLevel - 1].GetComponentInChildren<Renderer>();

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

    public void Select() {
        selectionCircle.SetActive(true);
    }

    public void DeSelect() {
        selectionCircle.SetActive(false);
        SetTeam(team);
    }
}
