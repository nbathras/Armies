﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    private Rigidbody rb;

    private float initializationTime;

    public float despawnTime;
    public Vector3 initialVelocity = new Vector3(0,0,0);

    private void Start()
    {
        initializationTime = Time.timeSinceLevelLoad;

        rb = GetComponent<Rigidbody>();

        transform.position += new Vector3(
            Random.Range(0, 10) / 100f - .05f,
            Random.Range(0, 10) / 100f - .05f,
            Random.Range(0, 10) / 100f - .05f
        );

        rb.velocity = initialVelocity + new Vector3(0, -10, 0);
    }

    private bool updateRotation = true;

    private void Update()
    {
        float timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

        if (timeSinceInitialization > despawnTime) {
            Destroy(gameObject);
        }

        if (updateRotation) {
            transform.forward = -rb.velocity;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Environment") {
            updateRotation = false;
            rb.isKinematic = true;
        }
    }
}