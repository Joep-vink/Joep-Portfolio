using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActionData : MonoBehaviour
{
    [field: SerializeField]
    public bool Attack { get; set; }        //If your attacking
    [field: SerializeField]
    public bool TargetSpotted { get; set; } //If you spotted a target
    [field: SerializeField]
    public bool Arrived { get; set; }       //If you arrive to the target
}
