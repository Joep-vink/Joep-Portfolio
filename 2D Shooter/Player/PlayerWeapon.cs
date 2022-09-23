using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : AgentWeapon
{
    public static PlayerWeapon instance;

    public GameObject weapenPos;

    public UIAmmo UIAmmo = null;

    public List<Weapon> availableGuns = new List<Weapon>();
    public int currentGun;

    public bool AmmoFull { get => availableGuns[currentGun] != null && availableGuns[currentGun].AmmoFull;}

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UIAmmo = FindObjectOfType<UIAmmo>();
        SwitchGun();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && availableGuns.Count > 0 && !availableGuns[currentGun].isReloadingCoroutine && !availableGuns[currentGun].isShooting && !availableGuns[currentGun].reloadCoroutine)
        {
            AgentInput.instance.OnFireButtonPressed.RemoveListener(availableGuns[currentGun].TryShooting);
            AgentInput.instance.OnFireButtonReleased.RemoveListener(availableGuns[currentGun].StopShooting);
            AgentInput.instance.OnReloadButtenPressed.RemoveListener(availableGuns[currentGun].TryReload);
            currentGun++;
            if (currentGun >= availableGuns.Count)
            {
                currentGun = 0;
            }
            SwitchGun();
        }
    }

    public void UpdateAmmoUI()
    {
        if (availableGuns[currentGun] != null)
        {
            availableGuns[currentGun].OnMaxAmmoChange.AddListener(UIAmmo.UpdateMaxAmmo);
            UIAmmo.UpdateMaxAmmo(availableGuns[currentGun].MaxAmmo);

            availableGuns[currentGun].OnAmmoChange.AddListener(UIAmmo.UpdateBulletsText);
            UIAmmo.UpdateBulletsText(availableGuns[currentGun].Ammo);
        }
    }

    public void AddAmmo(float amount)
    {
        if (availableGuns[currentGun] != null)
        {
            weapon.MaxAmmo += amount;
        }
    }

    public void SwitchGun()
    {
        foreach (Weapon gun in availableGuns)
        {
            gun.gameObject.SetActive(false);
        }
        availableGuns[currentGun].gameObject.SetActive(true);

        AssignWeapon();
        UpdateAmmoUI();

        AgentInput.instance.OnFireButtonPressed.AddListener(availableGuns[currentGun].TryShooting);
        AgentInput.instance.OnFireButtonReleased.AddListener(availableGuns[currentGun].StopShooting);
        AgentInput.instance.OnReloadButtenPressed.AddListener(availableGuns[currentGun].TryReload);

        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunName.text = availableGuns[currentGun].weaponName;
    }
}
