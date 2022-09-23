using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake() => instance = this;

    public Transform parentObject;

    public int DefenderHealth, EnemyHealth, Currency;

    [field: SerializeField]
    public UnityEvent OnDie;

    [field: SerializeField]
    public UnityEvent OnWin;

    public bool UseCheats;

    [HideInInspector] public bool isDead, won;

    [HideInInspector] public bool IsUpgrading;

    [Header("Path")]
    public WayPoints path;
    public float followPathRadius = 1;

    private void Update()
    {
        if (UseCheats)
        {
            Currency = 999;
            DefenderHealth = 999;
        }
    }
}
