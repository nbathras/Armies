using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static GameManager;

public class Building : MonoBehaviour
{
    public int maxNumberOfUnits = 100;
    public int troopGenerationRate = 10;

    public Team team = Team.Netural;
    public TextMeshPro troopNumberText;
    public GameObject selectionCircle;

    private Renderer modelRenderer;

    private void Awake() {
        modelRenderer = gameObject.GetComponentInChildren<Renderer>();

        selectionCircle.SetActive(false);
        troopNumberText.SetText("40");
        SetTeam(team);

        StartCoroutine(GenerateUnits());
    }

    private IEnumerator GenerateUnits() {
        while(true) {
            yield return new WaitForSeconds(1f);

            if (team != Team.Netural) {
                int troopNumber = GetTroopNumber();

                troopNumber += troopGenerationRate;

                if (troopNumber > maxNumberOfUnits) {
                    troopNumber = maxNumberOfUnits;
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

            maxNumberOfUnits += 100;
            troopGenerationRate += 10;

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
