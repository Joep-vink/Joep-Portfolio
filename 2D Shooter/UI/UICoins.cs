using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICoins : MonoBehaviour
{
    public static UICoins instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private TextMeshProUGUI coinText = null;

    public void UpdateCoins(float coinsAmount)
    {
        coinText.SetText(coinsAmount.ToString());
    }
}
