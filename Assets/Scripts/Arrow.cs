using UnityEngine;

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

        rb.velocity = initialVelocity;
    }

    private void Update()
    {
        float timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

        if (timeSinceInitialization > despawnTime) {
            Destroy(gameObject);
        }

        if (transform.position.y <= .1f) {
            rb.isKinematic = true;
        } else {
            transform.forward = -rb.velocity;
        }
    }
}
