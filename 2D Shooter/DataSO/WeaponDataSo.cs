using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/WeaponData")]
public class WeaponDataSo : ScriptableObject
{
    [field: SerializeField]
    public BulletDataSo bulletData { get; set; }

    [field: SerializeField]
    [field: Range(0, 100)]
    public int AmmoMagCapacity { get; set; } = 100;

    [field: SerializeField]
    [field: Range(0, 200)]
    public int MaxAmmo { get; set; } = 200;

    [field: SerializeField]
    public bool AutomaticFire { get; set; } = false;

    [field: SerializeField]
    [field: Range(0.01f, 2f)]
    public float WeaponDelay { get; set; } = 0.1f;

    [field: SerializeField]
    [field: Range(0.01f, 10f)]
    public float WeaponReload { get; set; } = 1f;

    [field: SerializeField]
    [field: Range(0, 20)]
    public float SpreadAngle { get; set; } = 5;

    [SerializeField] private bool multiBulletShoot = false;
    [Range(1, 10)]
    [SerializeField] private int bulletCount = 1;

    internal int GetBulletCountToSpawn()
    {
        if (multiBulletShoot)
        {
            return bulletCount;
        }
        return 1;
    }
}
