using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private WeaponHolder weaponHolder;
    [SerializeField] private float speed = 5;

    public void Move(Vector2 direction)
    {
        Weapon currentWeapon = weaponHolder.GetCurrentWeapon();
        float currentWeaponWeight = currentWeapon != null ? currentWeapon.weight : 0f;

        float weaponMovementCoeff = 1f - currentWeaponWeight * 0.01f;
        PerformMovement(direction, weaponMovementCoeff);
    }

    private void PerformMovement(Vector2 direction, float coeff)
    {
        Vector3 moveVector = new Vector3(direction.x, 0, direction.y);
        characterController.Move(moveVector * speed * coeff);
    }
}
