using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{

    private Rigidbody rb;

    private float initializationTime;
    private bool updateRotation = true;

    public float despawnTime;
    public Vector3 initialVelocity = new Vector3(0,0,0);
    public ArrowStorm arrowStorm;

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
        if (collision.gameObject.CompareTag("Unit")) {
            arrowStorm.HitUnit(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Environment")) {
            updateRotation = false;
            rb.isKinematic = true;
        }
    }
}
