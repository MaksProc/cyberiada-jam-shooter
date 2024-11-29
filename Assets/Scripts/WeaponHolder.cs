using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private List<Weapon> weapons; // Inventory of weapons
    [SerializeField] private int currentWeaponIndex;
    [SerializeField] private bool isHeldByPlayer;

    private void Start()
    {
        if (weapons.Count > 0)
        {
            EquipWeapon(0); // Equip the first weapon by default
        }
    }

    public void Attack() => weapons[currentWeaponIndex]?.Attack();

    public void Reload()
    {
        if (weapons[currentWeaponIndex] is IReloadable reloadableWeapon)
            reloadableWeapon.Reload();
    }

    public void SwitchWeaponByIndex(int index)
    {
        if (index < 0 || index >= weapons.Count) return; // Validate index
        if (currentWeaponIndex == index) return;         // Ignore if already equipped

        EquipWeapon(index);
    }

    private void EquipWeapon(int index)
    {
        // Deactivate the current weapon
        weapons[currentWeaponIndex]?.gameObject.SetActive(false);

        // Activate the new weapon
        currentWeaponIndex = index;
        weapons[currentWeaponIndex].isHeldByPlayer = true;
        weapons[currentWeaponIndex].gameObject.SetActive(true);
        weapons[currentWeaponIndex].isHeldByPlayer = isHeldByPlayer;
        if (isHeldByPlayer)
            EventManager.Instance.Weapon.OnWeaponEquip.Invoke(weapons[currentWeaponIndex]);
    }

    public Weapon GetCurrentWeapon()
    {
        if (currentWeaponIndex < 0 || currentWeaponIndex >= weapons.Count)
            return null;

        return weapons[currentWeaponIndex];
    }
}

