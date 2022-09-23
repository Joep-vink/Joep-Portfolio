using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class player : MonoBehaviour, IAgent, IHittable, IKnockBack
{
    public static player instance;

    public float maxHealth;
    public float health;

    public GameObject particleCollider; 

    public float Health { get => health; set { health = Mathf.Clamp(value, 0, maxHealth); UiHealth.UpdateUI(Health); } }

    private bool dead = false;
    private PlayerWeapon playerWeapon;
    private AgentMovement agentMovement;

    [field: SerializeField]
    public UIHealth UiHealth { get; set; }

    [field: SerializeField]
    public UnityEvent OnDie { get; set; }

    [field: SerializeField]
    public UnityEvent OnGetHit { get; set; }

    private void Awake()
    {
        instance = this;

        playerWeapon = GetComponentInChildren<PlayerWeapon>();
    }

    private void Start()
    {
        UiHealth = FindObjectOfType<UIHealth>();
        agentMovement = GetComponent<AgentMovement>();

        CharacterTracker.instance.maxHealth = maxHealth;
        CharacterTracker.instance.currentHealth = health;

        UiHealth.UpdateUI(health);
    }

    public void GetHit(float damage, GameObject damageDealer)
    {
        if (!dead)
        {
            Health -= damage;
            OnGetHit?.Invoke();
            if (Health <= 0)
            {
                OnDie?.Invoke();
                dead = true;
                UIController.instance.deathMenu.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Resource"))
        {
            var resource = coll.gameObject.GetComponent<Resource>();
            if (resource != null)
            {
                switch (resource.ResourceData.ResourceType)
                {
                    case ResourceTypeEnum.Coin:
                        GameManager.instance.GetCoins(resource.ResourceData.GetAmount());
                        resource.PickUpResource();
                        break;
                    case ResourceTypeEnum.Health:
                        if (Health >= maxHealth)
                        {
                            health = maxHealth;
                            return;
                        }
                        health += resource.ResourceData.GetAmount();
                        resource.PickUpResource();
                        if (Health >= maxHealth)
                        {
                            health = maxHealth;
                        }
                        UiHealth.UpdateUI(health);
                        break;
                    case ResourceTypeEnum.Ammo:
                        if (playerWeapon.AmmoFull)
                        {
                            return;
                        }
                        playerWeapon.AddAmmo(resource.ResourceData.GetAmount());
                        resource.PickUpResource();
                        break;
                    case ResourceTypeEnum.Gun:
                        GunPickup.instance.GiveGun();
                        resource.PickUpResource();
                        break;
                }
            }
        }
    }

    public void KnockBack(Vector2 direction, float power, float duration)
    {
        agentMovement.KnockBack(direction, power, duration);
    }
}
