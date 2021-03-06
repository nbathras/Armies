﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private LayerMask buildingtMask;
    [SerializeField]
    private GameObject powerCircle;
    [SerializeField]
    private LayerMask environmentMask;

    [SerializeField]
    private UIController uIController;

    private Camera cam;

    private Building selected;

    private InputStates currentState;
    private bool mouseButton0Clicked, mouseButton1Clicked;

    private enum InputStates
    {
        Default,
        BuildingSelected,
        ArrowPowerSelected,
    }

    private void Start() {
        cam = Camera.main;

        currentState = InputStates.Default;

        selected = null;

        mouseButton0Clicked = false;
        mouseButton1Clicked = false;
    }

    private void Update() {
        switch (currentState) {
            case InputStates.Default:
                if (Input.GetMouseButtonDown(0) && !mouseButton0Clicked) {
                    mouseButton0Clicked = true;

                    ClearSelected();

                    if (SelectBuilding()) {
                        currentState = InputStates.BuildingSelected;
                    } else {
                        currentState = InputStates.Default;
                    }
                } else if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    ClearSelected();

                    currentState = InputStates.ArrowPowerSelected;
                }

                break;
            case InputStates.BuildingSelected:
                if (Input.GetMouseButton(0) && !mouseButton0Clicked) {
                    mouseButton0Clicked = true;

                    ClearSelected();

                    currentState = InputStates.Default;
                } else if (Input.GetMouseButtonDown(1) && !mouseButton1Clicked) {
                    mouseButton1Clicked = true;

                    FocusBuilding();

                    ClearSelected();

                    currentState = InputStates.Default;
                } else if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    ClearSelected();

                    currentState = InputStates.ArrowPowerSelected;
                }
                break;
            case InputStates.ArrowPowerSelected:
                powerCircle.SetActive(true);

                powerCircle.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                uIController.toggleArrow(true);

                if (Input.GetMouseButton(1)) {
                    SpawnArrowStorm();

                    powerCircle.SetActive(false);

                    currentState = InputStates.Default;
                    uIController.toggleArrow(false);

                } else if (Input.GetMouseButtonDown(0)) {
                    powerCircle.SetActive(false);

                    currentState = InputStates.Default;
                    uIController.toggleArrow(false);
                }

                break;
        }

        if (Input.GetMouseButtonUp(0)) { mouseButton0Clicked = false; }
        if (Input.GetMouseButtonUp(1)) { mouseButton1Clicked = false; }
    }

    private bool SelectBuilding() {
        if (DetectMaskHit(out RaycastHit hit, buildingtMask)) {
            selected = hit.collider.gameObject.GetComponent<Building>();

            if (selected.GetTeam().Equals(GameManager.instance.GetPlayerControlledTeam())) {
                selected.Select();
                return true;
            } else {
                selected = null;
            }
        }
        return false;
    }

    private bool FocusBuilding() {
        if (DetectMaskHit(out RaycastHit hit, buildingtMask)) {
            Building focused = hit.collider.gameObject.GetComponent<Building>();
            if (focused == selected && selected.GetTeam().Equals(GameManager.instance.GetPlayerControlledTeam())) {
                selected.Upgrade();
            } else {
                Unit.ConstructUnit(selected, hit.collider.gameObject.GetComponent<Building>());
            }

            return true;
        }

        return false;
    }

    private bool DetectMaskHit(out RaycastHit hit, LayerMask mask) {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, 100, mask);
    }

    private bool SpawnArrowStorm() {
        int currentPlayerGold = GameManager.instance.GetPlayerControlledTeam().GetGold();

        if (currentPlayerGold >= 25) {
            GameManager.instance.GetPlayerControlledTeam().SetGold(currentPlayerGold - 25);

            Vector3 target = new Vector3(0, 0, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, environmentMask)) {
                target = hit.point;
            }

            ArrowStorm arrowStorm = Instantiate(Blueprints.ArrowStormStaticPrefab).GetComponent<ArrowStorm>();
            arrowStorm.Initalize(target);

            return true;
        }
        return false;
    }

    private void ClearSelected() {
        if (selected != null) {
            selected.DeSelect();
            selected = null;
        }
    }

    public void SelectArrowStorm()
    {
        currentState = InputStates.ArrowPowerSelected;
    }
}
