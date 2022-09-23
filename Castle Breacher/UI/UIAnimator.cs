using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private bool cardBoxUp = true;

    public void ChangeCardBoxState(string boolName)
    {
        if (cardBoxUp)
        {
            cardBoxUp = false;
            anim.SetBool(boolName, false);
        }
        else
        {
            cardBoxUp = true;
            anim.SetBool(boolName, true);
        }
    }
}
