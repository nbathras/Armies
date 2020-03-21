using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    public LayerMask buildingtMask;

    private Camera cam;

    private bool isMouse0clicked, isMouse1clicked;

    private Building selected;

    private void Start() {
        cam = Camera.main;

        isMouse0clicked = false;
        isMouse1clicked = false;

        selected = null;
    }

    private void Update() {
        if (selected != null && selected.team != "red") {
            ClearSelected();
        }

        if (Input.GetMouseButtonDown(0) && !isMouse0clicked) {
            isMouse0clicked = true;

            ClearSelected();

            if (DetectBuildingHit(out RaycastHit hit)) {
                selected = hit.collider.gameObject.GetComponent<Building>();

                if (selected.team == "red") {
                    selected.Select();
                } else {
                    selected = null;
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && !isMouse1clicked) {
            isMouse1clicked = true;

            if (selected != null && DetectBuildingHit(out RaycastHit hit)) {
                Building focused = hit.collider.gameObject.GetComponent<Building>();
                if (focused == selected && selected.team == "red") {
                    selected.Upgrade();
                } else {
                    Unit.ConstructUnit(selected, hit.collider.gameObject.GetComponent<Building>());
                }
            }

            ClearSelected();
        }

        if (Input.GetMouseButtonUp(0)) {
            isMouse0clicked = false;
        }
        if (Input.GetMouseButtonUp(1)) {
            isMouse1clicked = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
    }

    private bool DetectBuildingHit(out RaycastHit hit) {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, 100, buildingtMask);
    }

    private void ClearSelected() {
        if (selected != null) {
            selected.DeSelect();
            selected = null;
        }
    }
}
