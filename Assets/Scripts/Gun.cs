using UnityEngine;
using System.Collections;

public class Gun : Weapon, IReloadable
{
    [Header("Gun Settings")]
    [SerializeField] private GameObject muzzleParticlePrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shotAudioPrefab;
    [SerializeField] private Transform muzzleTransf;
    [SerializeField] private WeaponMovementHandler weaponMovementHandler;

    // Ammo Settings
    [field: SerializeField] public int magSize { get; private set; }
    [field: SerializeField] public int currentMagAmmo { get; private set; }
    [field: SerializeField] public int currentAmmo { get; private set; }
    [field: SerializeField] public float reloadTime { get; private set; }

    // Gun Settings
    [field: SerializeField] public float bulletSpeed { get; private set; }
    [field: SerializeField] public float baseSpreadAngle { get; private set; } = 2f; // Base spread angle in degrees
    [field: SerializeField] public float maxSpreadAngle { get; private set; } = 10f; // Max spread angle
    [field: SerializeField] public float spreadIncreasePerShot { get; private set; } = 1f; // Increase in spread per shot
    [field: SerializeField] public float spreadRecoverySpeed { get; private set; } = 2f; // Speed at which spread recovers
    [field: SerializeField] public float movementSpreadCoeff { get; private set; } = 0.1f;

    public float currentSpreadAngle { get; private set; } // Tracks the current spread angle
    private bool reloading;


    private void Start()
    {
        currentSpreadAngle = baseSpreadAngle; // Initialize with the base spread angle
    }

    public override void Attack()
    {
        if (CanAttack() && !reloading && currentMagAmmo > 0)
        {
            lastAttackTime = Time.time;

            // Muzzle flash
            GameObject particleGO = Instantiate(muzzleParticlePrefab, muzzleTransf.position, muzzleTransf.rotation);
            var particleSystem = muzzleParticlePrefab.GetComponent<ParticleSystem>().main;
            Destroy(particleGO, particleSystem.duration + particleSystem.startLifetime.constantMax);

            // Shot sound
            GameObject audioGO = Instantiate(shotAudioPrefab, muzzleTransf.position, muzzleTransf.rotation);
            Destroy(audioGO, shotAudioPrefab.GetComponent<AudioSource>().clip.length);

            // Calculate spread based on the weapon's velocity
            Quaternion spreadRotation = CalculateSpread();

            // Bullet
            GameObject bulletGO = Instantiate(bulletPrefab, muzzleTransf.position, spreadRotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.SetValues(bulletSpeed, damage, range);

            currentMagAmmo--;

            // Increase spread for consecutive shots
            currentSpreadAngle = Mathf.Min(currentSpreadAngle + spreadIncreasePerShot, maxSpreadAngle);

            if (isHeldByPlayer) EventManager.Instance.Weapon.OnPlayerAttackEnd.Invoke(this);
        }
    }

    private void Update()
    {
        UpdateMovementSpread();
        RecoverSpread();
    }

    private Quaternion CalculateSpread()
    {
        // Apply the total spread to the muzzle's forward direction
        float randomYaw = Random.Range(-currentSpreadAngle / 2, currentSpreadAngle / 2); // Horizontal spread

        return Quaternion.Euler(muzzleTransf.eulerAngles + new Vector3(0, randomYaw, 0));
    }

    private void RecoverSpread()
    {
        if (currentSpreadAngle > baseSpreadAngle)
        {
            currentSpreadAngle = Mathf.Max(currentSpreadAngle - spreadRecoverySpeed * Time.deltaTime, baseSpreadAngle);
        }
    }

    private void UpdateMovementSpread()
    {
        Vector3 weaponVelocity = weaponMovementHandler ? weaponMovementHandler.GetWeaponVelocity() : Vector3.zero;
        float velocityMagnitude = weaponVelocity.magnitude;

        float movementSpread = velocityMagnitude * Time.deltaTime * maxSpreadAngle * movementSpreadCoeff;

        currentSpreadAngle = Mathf.Min(currentSpreadAngle + movementSpread, maxSpreadAngle);
    }

    public void Reload()
    {
        if (!reloading && currentAmmo > 0 && currentMagAmmo < magSize)
            StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        if (isHeldByPlayer) EventManager.Instance.Weapon.OnReloadStart.Invoke();
        reloading = true;

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = magSize - currentMagAmmo;
        int ammoToReload = Mathf.Min(currentAmmo, ammoNeeded);

        currentMagAmmo += ammoToReload;
        currentAmmo -= ammoToReload;

        if (isHeldByPlayer) EventManager.Instance.Weapon.OnReloadEnd.Invoke(this);
        reloading = false;
    }

    private void OnDisable()
    {
        if (isHeldByPlayer) EventManager.Instance.Weapon.OnReloadEnd?.Invoke(this);
        reloading = false;
    }
}
