using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprints : MonoBehaviour
{
    private static Blueprints instance;

    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private GameObject dustParticlePrefab;
    [SerializeField]
    private GameObject arrowPrefab;

    public static GameObject UnitStaticPrefab {
        get {
            return instance.unitPrefab;
        }
    }

    public static GameObject DustParticleStaticPrefab {
        get {
            return instance.dustParticlePrefab;
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
