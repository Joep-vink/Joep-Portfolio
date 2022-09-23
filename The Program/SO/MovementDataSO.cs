using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/MovementData")]
public class MovementDataSO : ScriptableObject
{
    [Range(0.1F, 10)]
    public float maxSpeed = 5F;

    [Range(0.01f, 200)]
    public float sensitivity = 5f;

    [Range(0.01f, 2)]
    public float acceleration;

    [Range(0.01f, 2)]
    public float deacceleration;
}
