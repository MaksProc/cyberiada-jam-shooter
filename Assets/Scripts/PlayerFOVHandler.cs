using UnityEngine;

public class PlayerFOVHandler : MonoBehaviour
{
    [SerializeField] private WeaponHolder weaponHolder;
    [SerializeField] private FieldOfView fieldOfView;

    private void Update()
    {
        Weapon weapon = weaponHolder.GetCurrentWeapon();
        fieldOfView.setViewDistance(weapon.range);

        if (weapon is Gun gun)
        {
            fieldOfView.UpdateFOV(gun.currentSpreadAngle);
        }
        else if (weapon is MeleeWeapon meleeWeapon)
        {
            fieldOfView.UpdateFOV(1f);
        }
    }
}
