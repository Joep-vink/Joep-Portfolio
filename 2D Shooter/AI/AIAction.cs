using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction : MonoBehaviour
{
    public AIActionData aiActionData;
    protected AIMovementData aiMovementData;
    protected EnemyAiBrain enemyAiBrain;

    private void Awake()
    {
        aiActionData = transform.root.GetComponentInChildren<AIActionData>();
        aiMovementData = transform.root.GetComponentInChildren<AIMovementData>();
        enemyAiBrain = transform.root.GetComponent<EnemyAiBrain>();
    }

    public abstract void TakeAction();
}
