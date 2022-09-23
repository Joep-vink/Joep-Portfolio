using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public Slider CountDownslider;
    public CardSO card;
    public int Lvl;

    [Header("CardInfo")]
    public TextMeshProUGUI DamageText;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI CostText;
    public Image AgentSprite;
    public Image UpgradeIcon;

    [HideInInspector] public SpawnManager Manager;
    [HideInInspector] public SpawnerInfo SpawnerInfo;
    private bool _onCooldown;
    [HideInInspector] public Animator anim;
    private float T = 0;

    public GameObject cantBuyPanel;

    private void Start()
    {
        anim = GetComponent<Animator>();
        //Set card info
        SetValues();
    }

    private void Update()
    {
        if (GameManager.instance.Currency < card.cost || Manager.AllWaves.Count >= 4)
        {
            cantBuyPanel.SetActive(true);
        }
        else
            cantBuyPanel.SetActive(false);

        if (_onCooldown)
        {
            CountDownslider.value = Mathf.Lerp(CountDownslider.maxValue, CountDownslider.minValue, T);
            T += card.timeBetweenBuy * Time.deltaTime;

            if (CountDownslider.value == 0)
            {
                T = 0;
                _onCooldown = false;
            }
        }
    }

    public void GiveInfoToSpawner()
    {
        if (GameManager.instance.IsUpgrading)
            UpdateCard();

        else if (!_onCooldown && GameManager.instance.Currency >= card.cost && Manager.AllWaves.Count < 4)
        {
            UiManager.instanace.UpdateCurrency(-card.cost);
            _onCooldown = true;
            Manager.AddWaves(card.spawnInfo);
            SpawnerInfo.spawner.waves.Add(card.spawnInfo);
        }
        else if (!_onCooldown && GameManager.instance.Currency < card.cost || Manager.AllWaves.Count >= 4 && !_onCooldown)
            anim.SetTrigger("CantClick");
    }

    private void SetValues()
    {
        AgentSprite.sprite = card.spr;

        DamageText.text = card.damage.ToString();
        HealthText.text = card.health.ToString();
        CostText.text = card.cost.ToString();
    }

    public void UpdateCard()
    {
        Lvl++;
        UpgradeManager.instance.AfterLevelUp();
    }
}
