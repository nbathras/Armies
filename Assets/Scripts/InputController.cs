using System.Collections.Generic;
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

    private Camera cam;

    private Building selected;

    private InputStates currentState;
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
    }

    private void Update() {
        switch (currentState) {
            case InputStates.Default:
                if (Input.GetMouseButtonDown(0)) {
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
                if (Input.GetMouseButtonDown(1)) {
                    FocusBuilding();

                    ClearSelected();

                    currentState = InputStates.Default;
                }
                break;
            case InputStates.ArrowPowerSelected:
                powerCircle.SetActive(true);

                powerCircle.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                if (Input.GetMouseButton(1)) {
                    SpawnArrowStorm();

                    powerCircle.SetActive(false);

                    currentState = InputStates.Default;

                } else if (Input.GetMouseButtonDown(0)) {
                    powerCircle.SetActive(false);

                    currentState = InputStates.Default;
                }

                break;
        }
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
        Vector3 target = new Vector3(0, 0, 0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, environmentMask)) {
            target = hit.point;
        }

        ArrowStorm arrowStorm = Instantiate(Blueprints.ArrowStormStaticPrefab).GetComponent<ArrowStorm>();
        arrowStorm.Initalize(target);

        /*
        Vector3 arrowStartingPosition = new Vector3(
            Camera.main.transform.position.x,
            5f,
            Camera.main.transform.position.z
        );
        Vector3 intialVelocity = new Vector3(hitPosition.x, 5f, hitPosition.z) - arrowStartingPosition;

        GameObject arrowStorm = Instantiate(Blueprints.ArrowStormStaticPrefab);
        arrowStorm.transform.position = arrowStartingPosition;

        List<ArrowStorm> arrowList = arrowStorm.GetComponentsInChildren<ArrowStorm>().ToList();
        foreach (Arrow arrow in arrowList) {
            arrow.initialForce = intialVelocity * 56;
        }
        */

        return true;
    }

    private void ClearSelected() {
        if (selected != null) {
            selected.DeSelect();
            selected = null;
        }
    }
}
