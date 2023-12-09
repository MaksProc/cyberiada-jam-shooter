using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Space(10)]
    public new string name;

    [Header("Movement")]
    public float weight;

    [Header("Fire")]
    public float damage;
    public float critChance;                //up to 100%
    public float critCoeff;
    public float fireRate;
    public float maxDistance;
    public float bulletSpeed;

    [Header("Scatter")]
    public float minScatter;
    public float maxScatter;                //scatter in euler angles 
    public float fireScatterPoints;         //points for 1 scatter unit (shot)
    public float motionScatterCoeff;        //multiply scatter points when person move
    public float currentScatterPoints;
    public float recovery;                  //restores scatter points in 1 second

    [Header("Ammo")]
    public int maxAmmo;
    public int currentAmmo;
    public int magSize;
    public int currentMagAmmo;

    [Header("Reloading")]
    public float reloadTime;

    [Space(20)]

    [SerializeField] GameObject particleSystempPrefab;
    [SerializeField] Bullet bullet; 
    [SerializeField] AudioSource shotAudio;
    [SerializeField] Transform muzzleTransf;

    [SerializeField] float particleSystemDestroyTime;

    private float timeBetweenShots;
    private float timeSinceLastShot;
    private bool reloadInput = true;

    private void Awake()
    {
        timeBetweenShots = 1 / (fireRate / 60);
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    [ContextMenu("Fire")]
    public void Fire()
    {
        if (currentMagAmmo > 0)
        {
            if (CanFire())
            {
                Debug.Log("Fire");

                GameObject partcleSystemGO = Instantiate(particleSystempPrefab, muzzleTransf.position, muzzleTransf.rotation);
                Destroy(partcleSystemGO, particleSystemDestroyTime);
                shotAudio.Play();

                //if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance))
                //{
                //    hit.transform.TryGetComponent<PersonHealth>(out PersonHealth hitPersonHealth);

                //    if (hitPersonHealth != null)
                //    {
                //        hitPersonHealth.GetDamage(damage);
                //    }
                //}

                Bullet currentBullet = Instantiate(bullet, muzzleTransf.position, muzzleTransf.rotation);

                currentBullet.transform.rotation = muzzleTransf.rotation;

                currentBullet.SetValues(bulletSpeed, maxDistance, damage);

                currentMagAmmo--;
                timeSinceLastShot = 0;
            }
        }
        else
        {
            Reload();
        }
    }

    private bool CanFire() => reloadInput && timeSinceLastShot >= timeBetweenShots;

    public IEnumerator Reload()
    {
        if (currentAmmo > 0)
        {
            reloadInput = false;

            yield return new WaitForSeconds(reloadTime);

            if (currentAmmo >= magSize)
            {
                currentMagAmmo = magSize;
                currentAmmo -= magSize;
            }
            else
            {
                currentMagAmmo = currentAmmo;
                currentAmmo = 0;
            }

            reloadInput = true;
        }
    }
}
