using UnityEngine;
using TMPro;

public class WeaponDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI reloadingTMP;
    [SerializeField] private TextMeshProUGUI weaponNameTMP;
    [SerializeField] private TextMeshProUGUI weaponAmmoTMP;

    private void Start()
    {
        weaponNameTMP.enabled = true;
        weaponAmmoTMP.enabled = true;

        EventManager.Instance.Weapon.OnReloadStart.AddListener(HandleOnReloadStart);
        EventManager.Instance.Weapon.OnReloadEnd.AddListener(HandleOnReloadEnd);
        EventManager.Instance.Weapon.OnWeaponEquip.AddListener(HandleOnWeaponEquip);
        EventManager.Instance.Weapon.OnPlayerAttackEnd.AddListener(HandleOnPlayerAttack);
    }

    private void HandleOnReloadStart()
    {
        reloadingTMP.enabled = true;
    }

    private void HandleOnReloadEnd(Weapon weapon)
    {
        reloadingTMP.enabled = false;

        SetAmmoText(weapon);
    }

    private void HandleOnWeaponEquip(Weapon weapon)
    {
        weaponNameTMP.SetText(weapon.weaponName); // Update the weapon name

        SetAmmoText(weapon);
    }

    private void HandleOnPlayerAttack(Weapon weapon)
    {
        SetAmmoText(weapon);
    }

    private void SetAmmoText(Weapon weapon)
    {
        if (weapon is Gun gun)
        {
            weaponAmmoTMP.SetText($"{gun.currentMagAmmo}/{gun.currentAmmo}");
        }
        else
        {
            weaponAmmoTMP.SetText("");
        }
    }
}