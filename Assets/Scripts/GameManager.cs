using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private AIController aiController;
    [SerializeField]
    private Blueprints bluePrints;

    public GameObject buildingContainer;
    private Building[] buildingList;

    private void Start() {
        buildingList = buildingContainer.gameObject.GetComponentsInChildren<Building>();


    }

    private void Update() {
        
    }
}
