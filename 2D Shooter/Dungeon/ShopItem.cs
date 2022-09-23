using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ShopType
{
    isHealthRestore,
    isHealthUpgrade,
    isWeapon
};

public class ShopItem : MonoBehaviour
{
    public ShopType shopType; 

    public GameObject buyMessage;

    private bool inBuyZone;

    public int itemCost, extraHearts;

    private player player;

    [field: SerializeField]
    public UIHealth uiHealth { get; set; }

    //Weapons
    public Weapon[] potentialGuns;
    private Weapon theGun;
    public SpriteRenderer gunSprite;
    public TextMeshProUGUI infoText;

    private void Start()
    {
        if (shopType == ShopType.isWeapon)
        {
            int selectedGun = Random.Range(0, potentialGuns.Length);
            theGun = potentialGuns[selectedGun];

            gunSprite.sprite = theGun.gunUI;
            infoText.text = theGun.weaponName + "\n - " + theGun.WeaponPrice + " Gold - ";
            itemCost = theGun.WeaponPrice;
        }

        uiHealth = FindObjectOfType<UIHealth>();
        player = FindObjectOfType<player>();
    }

    private void Update()
    {
        if (inBuyZone && Input.GetKeyDown(KeyCode.E) && GameManager.instance.coins >= itemCost)
        {
            GameManager.instance.SpendCoins(itemCost);

            switch (shopType)
            {
                case ShopType.isHealthRestore:

                    player.health = player.maxHealth;
                    uiHealth.UpdateUI(player.maxHealth);

                    break;
                case ShopType.isHealthUpgrade:

                    player.maxHealth += extraHearts;
                    uiHealth.AddLive(extraHearts);
                    uiHealth.UpdateUI(player.health);

                    break;
                case ShopType.isWeapon:

                    Weapon gunClone = Instantiate(theGun);
                    gunClone.transform.parent = PlayerWeapon.instance.weapenPos.transform;
                    gunClone.transform.position = PlayerWeapon.instance.weapenPos.transform.position;
                    gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    gunClone.transform.localScale = Vector3.one;

                    gunClone.gameObject.SetActive(false);
                    PlayerWeapon.instance.availableGuns.Add(gunClone);

                    break;
                default:
                    break;
            }

            gameObject.SetActive(false);
            inBuyZone = false;
            //play buy sound
        }
        else
        {
            //play not enough money sound 
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            buyMessage.SetActive(true);
            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            buyMessage.SetActive(false);
            inBuyZone = false;
        }
    }
}

