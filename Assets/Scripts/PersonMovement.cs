using UnityEngine;
public class PersonMovement : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] private float speed = 1;
    [SerializeField] private float speedCoeff = 1;

    public void Move(Vector2 direction)
    {
        characterController.Move((new Vector3(direction.x, -2, direction.y) * speed * speedCoeff * Time.deltaTime));
    }
}