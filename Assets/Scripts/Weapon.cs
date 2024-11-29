using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Info")]
    [HideInInspector] public bool isHeldByPlayer; // If the weapon is held by the player
    [field: SerializeField] public string weaponName { get; protected set; }
    [field: SerializeField] public float weight { get; protected set; } // Movement substraction coefficient in procents
    [field: SerializeField] public int damage { get; protected set; }
    [field: SerializeField] public float range { get; protected set; }

    [Header("Common Settings")]
    [field: SerializeField] public float attackCooldown { get; private set; } // Cooldown between attacks
    protected float lastAttackTime; // To track attack timing

    public abstract void Attack();

    protected bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }
}