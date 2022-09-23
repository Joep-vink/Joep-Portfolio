using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : AIAction
{
    private bool firstAttack = true;
    public float firstAttackWaitTime = 0.5f;

    public override void TakeAction()
    {
        if (firstAttack)
        {
            StartCoroutine(AttackIenumerator());
        }
        else
        {
            aiMovementData.Direction = Vector2.zero;
            aiMovementData.PointOfIntrest = enemyAiBrain.Target.transform.position;
            enemyAiBrain.Move(aiMovementData.Direction, aiMovementData.PointOfIntrest);
            aiActionData.Attack = true;
            enemyAiBrain.Attack();
        }
    }

    private IEnumerator AttackIenumerator()
    {
        aiMovementData.Direction = Vector2.zero;
        aiMovementData.PointOfIntrest = enemyAiBrain.Target.transform.position;
        enemyAiBrain.Move(aiMovementData.Direction, aiMovementData.PointOfIntrest);
        aiActionData.Attack = true;
        yield return new WaitForSeconds(firstAttackWaitTime);
        enemyAiBrain.Attack();
        firstAttack = false;
    }
}
