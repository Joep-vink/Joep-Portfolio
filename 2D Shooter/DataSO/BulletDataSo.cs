using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/BulletData")]
public class BulletDataSo : ScriptableObject
{
    public string EnemyLayer;

    [field: SerializeField]
    public GameObject BulletPrefab { get; set; }

    [field: SerializeField]
    [field: Range(1, 100)]
    public float BulletSpeed { get; internal set; } = 1;

    [field: SerializeField]
    [field: Range(0.1f, 10)]
    public float Damage { get; set; } = 1;

    [field: SerializeField]
    [field: Range(0f, 100)]
    public float Friction { get; internal set; } = 0;

    [field: SerializeField]
    public bool Bounce { get; set; } = false;

    [field: SerializeField]
    public bool GoTroughHittable { get; set; } = false;

    [field: SerializeField]
    public bool IsRaycast { get; set; } = false;

    [field: SerializeField]
    public GameObject ImpactObstaclePrefab { get; set; }

    [field: SerializeField]
    public GameObject ImpactEnemyPrefab { get; set; }

    [field: SerializeField]
    [field: Range(0, 20)]
    public float KnockBackPower { get; set; } = 5;

    [field: SerializeField]
    [field: Range(0f, 1)]
    public float KnockBackDelay { get; set; } = 0.01f;
}
