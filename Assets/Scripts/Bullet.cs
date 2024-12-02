using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private int damage;
    private float range;
    private float distanceTravelled;

    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    // Set values from the Weapon when instantiated
    public void SetValues(float speed, int damage, float range)
    {
        this.speed = speed;
        this.damage = damage;
        this.range = range;

        // Apply the speed to the bullet's initial velocity
        bulletRigidbody.linearVelocity = transform.forward * speed;
    }

    private void Update()
    {
        distanceTravelled += bulletRigidbody.linearVelocity.magnitude * Time.deltaTime;

        if (distanceTravelled >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
        }
        if (collision.gameObject.TryGetComponent(out EnemyMovement enemyMovement))
        {
            enemyMovement.TriggerBulletHitReaction();
        }
        Destroy(gameObject);
    }
}