using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    [SerializeField]
    private Vector3 initialForce;

    [SerializeField]
    private float despawnTime = 4f;

    private Rigidbody rb;
    private float initializationTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initializationTime = Time.timeSinceLevelLoad;
        despawnTime += Random.Range(0, 30) / 100.0f;
        transform.position += new Vector3(0, Random.Range(0, 2), 0);
        transform.Rotate(new Vector3(0, Random.Range(0, 360), 0), Space.World);

        rb.AddForce(initialForce);
    }

    private void Update() {
        float timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

        if (timeSinceInitialization > despawnTime) {
            Destroy(gameObject);
        }

        if (transform.position.y <= .1f) {
            rb.isKinematic = true;
        }
    }
}
