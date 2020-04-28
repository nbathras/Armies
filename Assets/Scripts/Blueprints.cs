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

    void Awake() {
        instance = this;
    }
}
