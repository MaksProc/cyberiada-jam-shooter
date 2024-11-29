using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee Settings")]
    [SerializeField] private GameObject attackEffect; // Visual effects for the attack

    public override void Attack()
    {
        if (!CanAttack()) return;

        lastAttackTime = Time.time;

        if (attackEffect != null)
        {
            GameObject effect = Instantiate(attackEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f); // Destroy effect after 1 second
        }

        // Perform the raycast-based attack
        RaycastHit[] hits = Physics.RaycastAll(
            transform.position,
            transform.forward,
            range
        );

        foreach (RaycastHit hit in hits)
        {
            // Check if the object has the correct tag
            if (hit.collider.CompareTag("Enemy"))
            {
                if (hit.collider.TryGetComponent(out Health health))
                {
                    health.TakeDamage(damage);
                }
            }
        }

        if (isHeldByPlayer) EventManager.Instance.Weapon.OnPlayerAttackEnd.Invoke(this);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the forward direction line
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * range);
    }
}