using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAttack : MonoBehaviour, IAttack
{
    private FireBall fireBall;
    private Transform spawnPos;

    public void AttackOnStart(AIBrain brain, AgentSO agentSo, AgentAnimations anim, ObjectPool pool)
    {
        var obj = pool.SpawnObject(spawnPos : spawnPos);

        if (obj == null) return;

        anim.AttackAnim();

        obj.GetComponent<AIBrain>().wavePointIndex = brain.wavePointIndex;
        obj.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * .5f;
    }

    public void Attack(AIBrain brain, AgentSO agentSo, AgentAnimations anim, ObjectPool pool)
    {
        anim.AttackAnim();

        var obj = pool.SpawnObject();
        fireBall = obj.GetComponent<FireBall>();

        fireBall.Shooter = transform.gameObject;
        fireBall.Target = brain.Target.gameObject;
    }
}
