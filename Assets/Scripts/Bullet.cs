using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject bulletGO;
    [SerializeField] Transform bulletTransf;
    [SerializeField] Bullet bullet;
    [SerializeField] Rigidbody bulletRigidbody;
    
    [SerializeField] ParticleSystem trackParticleSystem;
    [SerializeField] Light bulletLight;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] PhysicMaterial ricochetMaterial;

    private float speed { get; set; }
    private float maxDistance { get; set; }
    private float damage { get; set; }
    
    private float distancePassed = 0;
    private float currentSpeed;
    private float speedForRicochet;
    private Vector3 lastFixedUpdateVelocity;
    private Vector3 currentFixedUpdateVelocity;

    public float ricochetcoeff = 0.6f;

    private void Start()
    {
        bulletRigidbody.velocity = bulletTransf.forward * speed;
        currentSpeed = bulletRigidbody.velocity.magnitude;
        speedForRicochet = currentSpeed * ricochetcoeff;
        currentFixedUpdateVelocity = bulletRigidbody.velocity;
    }

    public void SetValues(float speed, float maxDistance, float damage) 
    {
        this.speed = speed;
        this.maxDistance = maxDistance;
        this.damage = damage;
    }

    void Update()
    {
        if (distancePassed < maxDistance)
        {
            float step = speed * Time.deltaTime;            
            distancePassed += step;
        }
        else 
        {
            StopAndDestroy();
        }
    }

    private void FixedUpdate()
    {
        currentSpeed = bulletRigidbody.velocity.magnitude;

        lastFixedUpdateVelocity = currentFixedUpdateVelocity;

        currentFixedUpdateVelocity = bulletRigidbody.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<PersonHealth>(out PersonHealth personHealth);

        if (personHealth != null)
        {
            Debug.Log("Папал!");
            personHealth.GetDamage(damage);
            Destroy(bulletGO);
        }
        else if (other.CompareTag("Obstacle"))
        {
            if (currentSpeed < speedForRicochet)
            {
                StopAndDestroy();
            }
            else
            {
                Vector3 vel = Vector3.zero;
                if (lastFixedUpdateVelocity.x != bulletRigidbody.velocity.x) 
                {
                    vel.x = -lastFixedUpdateVelocity.x;
                }
                if (lastFixedUpdateVelocity.y != bulletRigidbody.velocity.y)
                {
                    vel.y = -lastFixedUpdateVelocity.y;
                }
                if (lastFixedUpdateVelocity.z != bulletRigidbody.velocity.z)
                {
                    vel.z = -lastFixedUpdateVelocity.z;
                }

                bulletRigidbody.velocity = vel;

                //sphereCollider.material = ricochetMaterial;
            }
            Debug.Log("В стену :(");
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    else if (other.CompareTag("Obstacle"))
    //    {
            
    //    }
    //}

    private void StopAndDestroy() 
    {
        bulletLight.DOIntensity(0, trackParticleSystem.startLifetime);
        Destroy(bulletGO, trackParticleSystem.startLifetime);
        bulletRigidbody.velocity = Vector3.zero;
        bullet.enabled = false;
    }
}