using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static GameManager;

public class Building : MonoBehaviour
{
    public int maxNumberOfUnits = 100;
    public int troopGenerationRate = 10;

    public int currentLevel = 0;

    public GameObject[] buildingModelStages;

    public Team team = Team.Netural;
    public TextMeshPro troopNumberText;
    public GameObject selectionCircle;

    private Renderer modelRenderer;

    private void Awake() {
        selectionCircle.SetActive(false);
        troopNumberText.SetText("40");
        SetTeam(team);
        SetLevel(currentLevel);

        StartCoroutine(GenerateUnits());
    }

    private void SetLevel(int level) {
        currentLevel = level;
        if (currentLevel > buildingModelStages.Length - 1) {
            currentLevel = buildingModelStages.Length - 1;
        }

        for (int i = 0; i < buildingModelStages.Length; i++) {
            if (i == currentLevel) {
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

            if (team != Team.Netural) {
                int troopNumber = GetTroopNumber();

                troopNumber += troopGenerationRate * (currentLevel + 1);

                if (troopNumber > maxNumberOfUnits * (currentLevel + 1)) {
                    troopNumber = maxNumberOfUnits * (currentLevel + 1);
                }

                SetTroopNumber(troopNumber);
            }
        }
    }

    public int GetTroopNumber() {
        return Int32.Parse(troopNumberText.text);
    }

    public void SetTroopNumber(int troopNumber) {
        troopNumberText.SetText(troopNumber.ToString());
    }

    public bool Upgrade() {
        int currentTroopNumber = GetTroopNumber();
        if (currentTroopNumber > ((int) maxNumberOfUnits / 2)) {
            SetTroopNumber(currentTroopNumber - ((int) maxNumberOfUnits / 2));

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

        modelRenderer = buildingModelStages[currentLevel].GetComponentInChildren<Renderer>();

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
