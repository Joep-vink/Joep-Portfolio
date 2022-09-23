using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTracker : MonoBehaviour
{
    public static CharacterTracker instance;

    public float currentHealth, maxHealth;
    public int currentCoins;

    private void Awake()
    {
        instance = this;
    } 
}
