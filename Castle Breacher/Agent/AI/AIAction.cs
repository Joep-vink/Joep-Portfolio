using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction : MonoBehaviour
{
    protected AIActionData aIActionData;
    protected AIMovementData aIMovementData;
    protected AIBrain aiBrain;

    private void Awake()
    {
        //Set all data
        aIActionData = transform.root.GetComponentInChildren<AIActionData>();
        aIMovementData = transform.root.GetComponentInChildren<AIMovementData>();
        aiBrain = transform.root.GetComponent<AIBrain>();
    }

    /// <summary>
    /// Implement the action here
    /// </summary>
    public abstract void TakeAction();
}
