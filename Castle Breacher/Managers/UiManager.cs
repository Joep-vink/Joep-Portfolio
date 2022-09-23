using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instanace;

    private void Awake()
    {
        instanace = this;
    }

    public TextMeshProUGUI waveText, currencyText, healthText, enemyHealthText;

    public DialogueSO dialogueSo;

    private void Start()
    {
        UpdateCurrency(0);
        UpdateHealth(0, true);
        UpdateHealth(0, false);
    }

    /// <summary>
    /// /// Updates the health UI
    /// </summary>
    /// <param name="amount">the health minus the amount</param>
    /// <param name="changeEnemyHealth">true = damage to enemy : false = damage to player</param>
    public void UpdateHealth(int amount, bool enemyHealth)
    {
        if (enemyHealth)
        {
            if (GameManager.instance.EnemyHealth >= amount)
            {
                GameManager.instance.EnemyHealth -= amount;
                if (GameManager.instance.EnemyHealth <= 0)
                    GameManager.instance.OnWin?.Invoke();
            }

            enemyHealthText.text = string.Format("{0}", GameManager.instance.EnemyHealth);
        }
        else
        {
            if (GameManager.instance.DefenderHealth >= amount)
            {
                GameManager.instance.DefenderHealth -= amount;
                if (GameManager.instance.DefenderHealth <= 0)
                    GameManager.instance.OnDie?.Invoke();
            }

            healthText.text = string.Format("{0}", GameManager.instance.DefenderHealth);
        }
    }

    /// <summary>
    /// Updates the currenct UI
    /// </summary>
    /// <param name="amount">the currency plus the amount</param>
    public void UpdateCurrency(int amount)
    {
        GameManager.instance.Currency += amount;

        currencyText.text = string.Format("{0}", GameManager.instance.Currency);
    }
}
