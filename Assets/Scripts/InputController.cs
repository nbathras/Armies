using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    public LayerMask buildingtMask;
    public LayerMask unitMask;

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
        if (selected != null && !selected.GetTeam().Equals(GameManager.instance.GetPlayerControlledTeam())) {
            ClearSelected();
        }

        if (Input.GetMouseButtonDown(0) && !isMouse0clicked) {
            isMouse0clicked = true;

            ClearSelected();

            if (DetectMaskHit(out RaycastHit hit, buildingtMask)) {
                selected = hit.collider.gameObject.GetComponent<Building>();

                if (selected.GetTeam().Equals(GameManager.instance.GetPlayerControlledTeam())) {
                    selected.Select();
                } else {
                    selected = null;
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && !isMouse1clicked) {
            isMouse1clicked = true;

            RaycastHit hit;
            if (selected != null && DetectMaskHit(out hit, buildingtMask)) {
                Building focused = hit.collider.gameObject.GetComponent<Building>();
                if (focused == selected && selected.GetTeam().Equals(GameManager.instance.GetPlayerControlledTeam())) {
                    selected.Upgrade();
                } else {
                    Unit.ConstructUnit(selected, hit.collider.gameObject.GetComponent<Building>());
                }
            }

            if (DetectMaskHit(out hit, unitMask)) {
                // Debug.Log("Right click Unit!");

                Unit focused = hit.collider.gameObject.GetComponent<Unit>();
                List<Vector3> positions = new List<Vector3>();
                positions.Add(new Vector3(0, -0, 0));

                positions.Add(new Vector3(.1f, 0, -.1f));
                positions.Add(new Vector3(.0f, 0, -.1f));
                positions.Add(new Vector3(-.1f, 0, -.1f));

                positions.Add(new Vector3(.1f, 0, .1f));
                positions.Add(new Vector3(.0f, 0, .1f));
                positions.Add(new Vector3(-.1f, 0, .1f));

                positions.Add(new Vector3(-.1f, 0, .0f));
                positions.Add(new Vector3(.1f, 0, .0f));

                positions.Add(new Vector3(.2f, 0, .2f));
                positions.Add(new Vector3(.1f, 0, .2f));
                positions.Add(new Vector3(.0f, 0, .2f));
                positions.Add(new Vector3(-.1f, 0, .2f));
                positions.Add(new Vector3(-.2f, 0, .2f));

                positions.Add(new Vector3(.2f, 0, -.2f));
                positions.Add(new Vector3(.1f, 0, -.2f));
                positions.Add(new Vector3(.0f, 0, -.2f));
                positions.Add(new Vector3(-.1f, 0, -.2f));
                positions.Add(new Vector3(-.2f, 0, -.2f));

                positions.Add(new Vector3(.2f, 0, .1f));
                positions.Add(new Vector3(.2f, 0, .0f));
                positions.Add(new Vector3(.2f, 0, -.1f));

                positions.Add(new Vector3(-.2f, 0, .1f));
                positions.Add(new Vector3(-.2f, 0, .0f));
                positions.Add(new Vector3(-.2f, 0, -.1f));

                for (int i = 0; i < positions.Count; i++) {
                    GameObject arrow = Instantiate(Blueprints.ArrowStaticPrefab);
                    arrow.transform.position = focused.transform.position + new Vector3(0, 2, 0) + positions[i] + (focused.transform.forward * .65f);
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
    }

    private bool DetectMaskHit(out RaycastHit hit, LayerMask mask) {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, 100, mask);
    }

    private void ClearSelected() {
        if (selected != null) {
            selected.DeSelect();
            selected = null;
        }
    }
}
