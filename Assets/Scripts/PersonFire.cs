using System;
using UnityEngine;

public class PersonFire : MonoBehaviour
{
    public Weapon currentWeapon; /*{ private get; set; }*/

    public void Fire() => currentWeapon.Fire();

    public void Reload() => StartCoroutine(currentWeapon.Reload());
}