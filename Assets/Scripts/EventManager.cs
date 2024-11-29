using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public class WeaponEvents
    {
        public UnityEvent<Weapon> OnWeaponEquip = new UnityEvent<Weapon>();
        public UnityEvent OnReloadStart = new UnityEvent();
        public UnityEvent<Weapon> OnReloadEnd = new UnityEvent<Weapon>();
        public UnityEvent<Weapon> OnPlayerAttackEnd = new UnityEvent<Weapon>();
    }

    public class HealthEvents
    {
        public UnityEvent<int> OnPlayerHealthChanged = new UnityEvent<int>();
        public UnityEvent OnPlayerDied = new UnityEvent();
    }

    public WeaponEvents Weapon = new WeaponEvents();
    public HealthEvents Health = new HealthEvents();
}