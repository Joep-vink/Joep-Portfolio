using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathMenu : MonoBehaviour
{
    public static DeathMenu instance;

    private void Awake()
    {
        instance = this;
    }
}
