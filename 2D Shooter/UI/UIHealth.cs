using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    [SerializeField]
    private GameObject heartPrefab = null, healthPanel = null;
    [SerializeField]
    private Sprite heartFull = null, heartHalf = null, heartEmpty = null;

    private float heartCount = 0;

    public List<Image> hearts = new List<Image>();

    public void Initialize(float livesCount)
    {
        heartCount = livesCount;
        foreach (Transform child in healthPanel.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < livesCount; i++)
        {
            hearts.Add(Instantiate(heartPrefab, healthPanel.transform).GetComponent<Image>());
        }
    }

    public void AddLive(float liveAmount)
    {
        heartCount += liveAmount;
        for (int i = 0; i < liveAmount; i++)
        {
            hearts.Add(Instantiate(heartPrefab, healthPanel.transform).GetComponent<Image>());
        }
    }

    public void UpdateUI(float health)
    {
        for (int i = 0; i < heartCount; i++)
        {
            if (i + 0.5f == health)
            {
                hearts[i].sprite = heartHalf;
            }
            else if (i < health)
            {
                hearts[i].sprite = heartFull;
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }
        }
    }
}
