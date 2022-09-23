using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IHittable, IAgent, IKnockBack
{
    [field: SerializeField]
    public EnemyDataSO EnemyData { get; set; }

    [field: SerializeField]
    public float Health { get; private set; } = 2;

    [field: SerializeField]
    public EnemyAttack enemyAttack { get; set; }

    private bool dead = false;

    private AgentMovement agentMovement;

    [field: SerializeField]
    public UnityEvent OnGetHit { get; set; }

    [field: SerializeField]
    public UnityEvent OnDie { get; set; }

    public bool canKnockBack = true;

    private void Awake()
    {
        if (enemyAttack == null)
        {
            enemyAttack = GetComponent<EnemyAttack>();
        }
        agentMovement = GetComponent<AgentMovement>();
    }

    private void Start()
    {
        Health = EnemyData.MaxHealth;
    }

    public void GetHit(float damage, GameObject damageDealer)
    {
        if (dead == false)
        {
            Health -= damage;
            OnGetHit?.Invoke();

            if (Health <= 0)
            {
                dead = true;
                OnDie?.Invoke();
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void PerformAttack()
    {
        if (dead == false)
        {
            enemyAttack.Attack(EnemyData.Damage);
        }
    }

    public void KnockBack(Vector2 direction, float power, float duration)
    {
        if (canKnockBack)
            agentMovement.KnockBack(direction, power, duration);
    }
}
