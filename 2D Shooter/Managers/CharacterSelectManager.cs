using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager instance;

    public player activePlayer;
    public CharacterSelector activeCharSelect;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        activeCharSelect.gameObject.SetActive(false);
    }
}
