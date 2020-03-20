using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprints : MonoBehaviour
{
    private static Blueprints instance;

    [SerializeField]
    private GameObject unitPrefab;

    public static GameObject unitStaticPrefab {
        get {
            return instance.unitPrefab;
        }
    }

    void Awake() {
        instance = this;
    }
}
