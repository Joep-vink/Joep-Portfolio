using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/CardData")]
public class CardSO : ScriptableObject
{
    [Header("CardInfo")]
    public Sprite spr;

    public int health, cost, damage;

    [Header("Settings")]
    public WaveSO spawnInfo;
    [Range(0.01f, 1f)]
    public float timeBetweenBuy;
}
