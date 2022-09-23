using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction : AIAction
{
    private bool firstAttack = true;
    public float firstAttackWaitTime = 0f;

    public override void TakeAction()
    {
        if (firstAttack)
        {
            StartCoroutine(AttackIenumerator());
        }
        else
        {
            var direction = enemyAiBrain.Target.transform.position - transform.position;
            aiMovementData.Direction = direction.normalized;
            aiMovementData.PointOfIntrest = enemyAiBrain.Target.transform.position;
            enemyAiBrain.Move(aiMovementData.Direction, aiMovementData.PointOfIntrest);
        }
    }

    private IEnumerator AttackIenumerator()
    {
        var direction = enemyAiBrain.Target.transform.position - transform.position;
        aiMovementData.Direction = direction.normalized;
        aiMovementData.PointOfIntrest = enemyAiBrain.Target.transform.position;
        yield return new WaitForSeconds(firstAttackWaitTime);
        enemyAiBrain.Move(aiMovementData.Direction, aiMovementData.PointOfIntrest);
        firstAttack = false;
    }
}
