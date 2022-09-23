using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    protected float desireAngle;

    [SerializeField] protected WeaponRenderer weaponRenderer;
    [SerializeField] protected Weapon weapon;

    private void Awake()
    {
        AssignWeapon();
    }

    public void AssignWeapon()
    {
        weaponRenderer = GetComponentInChildren<WeaponRenderer>();
        weapon = GetComponentInChildren<Weapon>();
    }

    public virtual void AimWeapon(Vector2 pointerPosition)
    {
        if (GameManager.instance.isPaused == false)
        {
            var aimDirection = (Vector3)pointerPosition - transform.position;
            desireAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            AdjustWeaponRendering();
            transform.rotation = Quaternion.AngleAxis(desireAngle, Vector3.forward);
        }
    }

    protected void AdjustWeaponRendering()
    {
        if (weaponRenderer != null || GameManager.instance.isPaused == false)
        {
            weaponRenderer?.FlipSprite(desireAngle > 90 || desireAngle < -90);
            weaponRenderer?.RenderBehindhead(desireAngle < 180 && desireAngle > 0);
        }
    }

    public void Shoot()
    {
        if (weapon != null || !GameManager.instance.isPaused || GameManager.instance.canShoot)
        {
            weapon.TryShooting();
        }
    }

    public void StopShooting()
    {
        if (weapon != null)
        {
           weapon.StopShooting();
        }
    }
}
