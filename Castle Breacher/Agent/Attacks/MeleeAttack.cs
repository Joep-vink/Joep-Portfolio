using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttack
{
    public void AttackOnStart(AIBrain brain, AgentSO agentSo, AgentAnimations anim, ObjectPool pool)
    {
    }

    public void Attack(AIBrain brain, AgentSO agentSo, AgentAnimations anim, ObjectPool pool)
    {
        anim.AttackAnim();

        var hittable = brain.Target.GetComponentInParent<IHittable>();
        hittable?.GetHit(agentSo.damage, gameObject);
    }
}