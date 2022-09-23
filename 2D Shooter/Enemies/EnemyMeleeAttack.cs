using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    public bool isBoss = false;

    public override void Attack(int damage)
    {
        if (isBoss && !waitBeforeNextAttack)
        {
            var hittable = player.instance.GetComponent<IHittable>();
            hittable?.GetHit(damage, gameObject);
            StartCoroutine(WaitBeforeAttackCoroutine());
        }
        else if (!waitBeforeNextAttack)
        {
            var hittable = GetTarget().GetComponent<IHittable>();
            hittable?.GetHit(damage, gameObject);
            StartCoroutine(WaitBeforeAttackCoroutine());
        }
    }
}
