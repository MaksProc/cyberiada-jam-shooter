using UnityEngine;

public class EnemyAwarenessTriggerInner : MonoBehaviour
{
    private EnemyMovement enemyMovement;

    void Start()
    {
        enemyMovement = this.GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { enemyMovement.TriggerChaseState(); }
    }
}
