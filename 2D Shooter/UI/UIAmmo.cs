using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammotext = null;
    [SerializeField] private TextMeshProUGUI maxAmmotext = null;

    [SerializeField] private Image defualtSprite;
    [SerializeField] private Sprite hasAmmoSprite, noAmmoSprite;

    [SerializeField]private bool outOfAmmo = false;

    public void UpdateMaxAmmo(float maxBulletCount)
    {
        if (maxBulletCount == 0)
        {
            maxAmmotext.color = Color.red;
            outOfAmmo = true;
        }
        else
        {
            maxAmmotext.color = Color.white;
            outOfAmmo = false;
            defualtSprite.sprite = hasAmmoSprite;
        }
        maxAmmotext.SetText(maxBulletCount.ToString());
    }

    public void UpdateBulletsText(float bulletCount)
    {
        if (bulletCount == 0)
        {
            ammotext.color = Color.red;
            if (outOfAmmo)
            {
                defualtSprite.sprite = noAmmoSprite;
            }
        }
        else
        {
            ammotext.color = Color.white;
        }
        ammotext.SetText(bulletCount.ToString());
    }
}
