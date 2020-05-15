using System.Collections;
using UnityEngine;

public class ArrowStorm: MonoBehaviour
{
    [SerializeField]
    private float startingHeight = 5.0f;
    [SerializeField]
    private float shotPower = 56.0f;

    [SerializeField]
    private float despawnTime = 4f;

    private float initializationTime;
    [SerializeField]
    private Arrow[] arrowArray;

    public void Initalize(Vector3 target) {
        initializationTime = Time.timeSinceLevelLoad;

        Vector3 arrowStartingPosition = new Vector3(
            Camera.main.transform.position.x,
            startingHeight,
            Camera.main.transform.position.z
        );
        transform.position = arrowStartingPosition;
        Vector3 initialVelocity = (new Vector3(target.x, startingHeight, target.z) - arrowStartingPosition); ;

        for (int i = 0; i < arrowArray.Length; i++) {
            arrowArray[i].initialVelocity = initialVelocity * shotPower;

            arrowArray[i].despawnTime = despawnTime - 1;
        }
    }

    private void Update() {
        float timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

        if (timeSinceInitialization > despawnTime) {
            Destroy(gameObject);
        }
    }
}
