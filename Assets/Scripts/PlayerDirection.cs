using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;

    // Direct the player to face a given direction vector
    public void Direct(Vector2 directionInput)
    {
        if (directionInput != Vector2.zero)
        {
            Vector3 directionVector = new Vector3(directionInput.x, 0, directionInput.y).normalized;
            DirectToPoint(transform.position + directionVector);
        }
    }

    // Direct the player to face a specific world point
    public void DirectToPoint(Vector3 targetPoint)
    {
        Vector3 direction = (targetPoint - transform.position);
        direction.y = 0; // Lock to XZ plane

        if (direction.sqrMagnitude > 0.01f) // Avoid no or small inputs
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }
}