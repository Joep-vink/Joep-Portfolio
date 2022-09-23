using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/ProjectileData")]
public class ProjectileSO : ScriptableObject
{
    public string targetLayer;
    public float speed;
    public int damage;
}
