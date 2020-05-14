using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprints : MonoBehaviour
{
    private static Blueprints instance;

    // unit blueprints
    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private Material redMaterial;
    [SerializeField]
    private Material yellowMaterial;
    [SerializeField]
    private Material greenMaterial;
    [SerializeField]
    private Material blueMaterial;

    // other blueprints
    [SerializeField]
    private GameObject arrowPrefab;

    public static GameObject UnitStaticPrefab {
        get {
            return instance.unitPrefab;
        }
    }

    public static Material RedStaticMaterial {
        get {
            return instance.redMaterial;
        }
    }

    public static Material YellowStaticMaterial {
        get {
            return instance.yellowMaterial;
        }
    }

    public static Material GreenStaticMaterial {
        get {
            return instance.greenMaterial;
        }
    }

    public static Material BlueStaticMaterial {
        get {
            return instance.blueMaterial;
        }
    }

    public static GameObject ArrowStaticPrefab {
        get {
            return instance.arrowPrefab;
        }
    }

    void Awake() {
        instance = this;
    }
}
