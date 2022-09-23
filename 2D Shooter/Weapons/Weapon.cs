using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject muzzle;

    [SerializeField] protected float ammo = 10;
    [SerializeField] protected float maxammo;

    [SerializeField] protected WeaponDataSo weaponData;

    public string weaponName;
    public Sprite gunUI;

    public int WeaponPrice;

    public float Ammo
    {
        get { return ammo; }
        set
        {
            ammo = Mathf.Clamp(value, 0, weaponData.AmmoMagCapacity);
            OnAmmoChange?.Invoke(ammo);
        }
    }

    public float MaxAmmo
    {
        get { return maxammo; }
        set
        {
            maxammo = Mathf.Clamp(value, 0, weaponData.MaxAmmo);
            OnMaxAmmoChange?.Invoke(maxammo);
        }
    }

    public bool AmmoFull { get => maxammo >= weaponData.MaxAmmo; }

    public bool isShooting = false;
    public bool reloadCoroutine = false;
    public bool isReloadingCoroutine = false;

    [field: SerializeField]
    public UnityEvent OnShoot { get; set; }

    [field: SerializeField]
    public UnityEvent OnShootNoAmmo { get; set; }

    [field: SerializeField]
    public UnityEvent<float> OnAmmoChange { get; set; }

    [field: SerializeField]
    public UnityEvent<float> OnMaxAmmoChange { get; set; }

    [field: SerializeField]
    public UnityEvent OnReload { get; set; }

    private void Start()
    {
        Ammo = weaponData.AmmoMagCapacity;
        maxammo = weaponData.MaxAmmo;
    }

    public void TryShooting()
    {
        isShooting = true;
    }

    public void StopShooting()
    {
        isShooting = false;
    }

    public void TryReload()
    {
        if (ammo < weaponData.AmmoMagCapacity && maxammo > 0 && isReloadingCoroutine == false && gameObject.activeInHierarchy)
        {
            StartCoroutine(ReloadDelay());
        }
    }

    protected IEnumerator ReloadDelay()
    {
        isReloadingCoroutine = true;
        OnReload?.Invoke();
        yield return new WaitForSeconds(weaponData.WeaponReload);
        Reload(weaponData.AmmoMagCapacity);
        isReloadingCoroutine = false;
    }

    public void Reload(int ammo)
    {
        var minAmmo = ammo - Ammo;

        if (maxammo < ammo)
        {
            Ammo += maxammo;
            maxammo -= minAmmo;
            if (maxammo < 0)
            {
                maxammo = 0;
            }
            OnMaxAmmoChange?.Invoke(maxammo);
        }
        else
        {
            maxammo -= minAmmo;
            Ammo += ammo;
            OnMaxAmmoChange?.Invoke(maxammo);
        }
    }

    private void Update()
    {
        UseWeapon();
    }

    private void UseWeapon()
    {
        if (isShooting && !reloadCoroutine && !isReloadingCoroutine && GameManager.instance.isAllowedToShoot)
        {
            if (Ammo > 0)
            {
                Ammo--;
                OnShoot?.Invoke();
                for (int i = 0; i < weaponData.GetBulletCountToSpawn(); i++)
                {
                    ShootBullet();
                }
            }
            else
            {
                isShooting = false;
                OnShootNoAmmo?.Invoke();
                return;
            }
            FinisfhShooting();
        }
    }

    private void FinisfhShooting()
    {
        StartCoroutine(DelayNextShootCoroutine());
        if (weaponData.AutomaticFire == false)
        {
            isShooting = false;
        }
    }

    protected IEnumerator DelayNextShootCoroutine()
    {
        reloadCoroutine = true;
        yield return new WaitForSeconds(weaponData.WeaponDelay);
        reloadCoroutine = false;
    }

    private void ShootBullet()
    {
        SpawnBullet(muzzle.transform.position, CalculateAngle(muzzle));
    }

    private void SpawnBullet(Vector3 position, Quaternion rotation)
    {
        var bulletPrefab = Instantiate(weaponData.bulletData.BulletPrefab, position, rotation);
        bulletPrefab.GetComponent<bullet>().BulletData = weaponData.bulletData;
    }

    private Quaternion CalculateAngle(GameObject muzzle)
    {
        float spread = Random.Range(-weaponData.SpreadAngle, weaponData.SpreadAngle);
        Quaternion bulletSpreadRotation = Quaternion.Euler(new Vector3(0, 0, spread));
        return muzzle.transform.rotation * bulletSpreadRotation;
    }
}
