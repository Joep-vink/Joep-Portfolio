using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    public int Minimum, Maximum, Current;
    public Image Mask;

    public Transform CardParent;

    public int[] Levels;

    public UnityEvent OnLevelUp;

    public UnityEvent OnNewLevel;

    private int _currentLevel;

    private void Start()
    {
        Maximum = Levels[_currentLevel];
        GetCurrentFill();
    }

    public void UpdateProgressBar(int xp)
    {
        UpdateXp(xp);
        GetCurrentFill();
    }

    private void UpdateXp(int xp)
    {
        Current += xp;

        if (Current >= Maximum)
        {
            _currentLevel++;
            Minimum = Maximum;
            Maximum = Levels[_currentLevel];
            OnLevelUp?.Invoke();
        }
    }

    private void GetCurrentFill()
    {
        float currentOffset = Current - Minimum;
        float maximumOffset = Maximum - Minimum;
        float fillAmount = currentOffset / maximumOffset;
        Mask.fillAmount = fillAmount;
    }

    public void OnClickUpgrade()
    {
        for (int i = 0; i < CardParent.childCount; i++)
        {
            if (CardParent.GetChild(i).TryGetComponent(out Card card))
            {
                if (card.Lvl < 3)
                {
                    card.UpgradeIcon.gameObject.SetActive(true);
                    //card.ProgressBar = this;
                }
            }
        }
        GameManager.instance.IsUpgrading = true;
    }

    public void AfterLevelUp()
    {
        for (int i = 0; i < CardParent.childCount; i++)
        {
            if (CardParent.GetChild(i).TryGetComponent(out Card card))
                card.UpgradeIcon.gameObject.SetActive(false);
        }
        OnNewLevel?.Invoke();
        GameManager.instance.IsUpgrading = false;
    }
}
