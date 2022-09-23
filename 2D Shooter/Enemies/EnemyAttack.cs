using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    protected EnemyAiBrain enemyBrain;

    [field: SerializeField]
    public float AttackDelay { get; private set; } = 1;
    protected bool waitBeforeNextAttack;


    private void Awake()
    {
        enemyBrain = GetComponent<EnemyAiBrain>();
    }

    public abstract void Attack(int damage);

    public IEnumerator WaitBeforeAttackCoroutine()
    {
        waitBeforeNextAttack = true;
        yield return new WaitForSeconds(AttackDelay);
        waitBeforeNextAttack = false;
    }

    protected GameObject GetTarget()
    {
        return enemyBrain.Target;
    }
}
