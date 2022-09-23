using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public static GunPickup instance;

    public Weapon gun;
    public    bool hasGun = false;
    
    private void Awake()
    {
        instance = this;
    }

    public void GiveGun()
    {

        foreach (Weapon gunToCheck in PlayerWeapon.instance.availableGuns)
        {
            if (gun.weaponName == gunToCheck.weaponName)
            {
                hasGun = true;
            }
        }

        if (!hasGun)
        {
            Weapon gunClone = Instantiate(gun);
            gunClone.transform.parent = PlayerWeapon.instance.weapenPos.transform;
            gunClone.transform.position = PlayerWeapon.instance.weapenPos.transform.position;
            gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
            gunClone.transform.localScale = Vector3.one;

            gunClone.gameObject.SetActive(false);
            PlayerWeapon.instance.availableGuns.Add(gunClone);
        }
    }
}
