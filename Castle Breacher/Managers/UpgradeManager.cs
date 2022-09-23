using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    private void Awake()
    {
        instance = this;
    }

    #region UpgradeCard
    public UnityEvent OnCardUpgrade;

    public Transform CardParent;

    /// <summary>
    /// Turn on animation and UpgradeIcon 
    /// </summary>
    public void UpgradeCard()
    {
        int c = 0; //the amount of upgradable cards

        for (int i = 0; i < CardParent.childCount; i++)
        {
            if (CardParent.GetChild(i).TryGetComponent(out Card card))
                if (card.Lvl < 3)
                {
                    c++;
                    card.UpgradeIcon.gameObject.SetActive(true);
                    card.anim.SetBool("IsUpgrading", true);
                }
        }

        if (c > 0) //If there are more than 0 cards
        {
            GameManager.instance.IsUpgrading = true;

            OnCardUpgrade?.Invoke();
        }
    }

    /// <summary>
    /// Turn off animation and UpgradeIcon 
    /// </summary>
    public void AfterLevelUp()
    {
        for (int i = 0; i < CardParent.childCount; i++)
        {
            if (CardParent.GetChild(i).TryGetComponent(out Card card))
            {
                card.anim.SetBool("IsUpgrading", false);
                card.UpgradeIcon.gameObject.SetActive(false);
            }
        }
        GameManager.instance.IsUpgrading = false;
    }

    #endregion

    #region UpgradeEconomy
    public UnityEvent OnEconomyUpgrade;

    public void UpgradeEconomy()
    {
        OnEconomyUpgrade?.Invoke();
        print("UpgradeEconomy");
    }

    #endregion

    #region UpgradeDefense
    public UnityEvent OnDefenseUpgrade;

    public void UpgradeDefense()
    {
        OnDefenseUpgrade?.Invoke();
        print("UpgradeDefense");
    }

    #endregion

    #region UpgradeSpawnTime
    public UnityEvent OnSpawnTimeUpgrade;

    public void UpgradeSpawnTime()
    {
        OnSpawnTimeUpgrade?.Invoke();
        print("UpgradeSpawnTime");
    }

    #endregion
}
