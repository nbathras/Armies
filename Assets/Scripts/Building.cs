using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Building : MonoBehaviour
{
    public static int MAX_UNIT_NUMBER = 200;
    public static int TROOP_GENERATION_RATE = 20;

    public string team = "neutral";
    public TextMeshPro troopNumberText;

    private Renderer modelRenderer;

    private void Awake() {
        modelRenderer = gameObject.GetComponentInChildren<Renderer>();

        troopNumberText.SetText("40");

        StartCoroutine(GenerateUnits());

        SetTeam(team);
    }

    private IEnumerator GenerateUnits() {
        while(true) {
            yield return new WaitForSeconds(1f);

            if (team != "neutral") {
                int troopNumber = GetTroopNumber();

                troopNumber += TROOP_GENERATION_RATE;

                if (troopNumber > MAX_UNIT_NUMBER) {
                    troopNumber = MAX_UNIT_NUMBER;
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

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Unit") {
            Unit unitObject = collision.gameObject.GetComponent<Unit>();

            if (unitObject.origin != this && unitObject.target == this) {
                int newTroopValue = -1;
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

    public void SetTeam(String inTeam) {
        team = inTeam;

        if (team.Equals("neutral")) {
            modelRenderer.material.color = Color.gray;
        } else {
            if (team.Equals("red")) {
                modelRenderer.material.color = Color.red;
            }
            if (team.Equals("blue")) {
                modelRenderer.material.color = Color.blue;
            }
        }
    }

    public void Select() {
        modelRenderer.material.color = Color.green;
    }

    public void DeSelect() {
        SetTeam(team);
    }
}
