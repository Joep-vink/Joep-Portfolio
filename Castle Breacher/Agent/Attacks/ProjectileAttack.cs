using System.Collections;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour, IAttack
{
    private Arrow arrow;

    public void AttackOnStart(AIBrain brain, AgentSO agentSo, AgentAnimations anim, ObjectPool pool)
    {
    }

    public void Attack(AIBrain brain, AgentSO agentSo, AgentAnimations anim, ObjectPool pool)
    {
        anim.AttackAnim();

        var obj = pool.SpawnObject();

        arrow = obj.GetComponent<Arrow>();
        
        arrow.Shooter = transform.gameObject;
        arrow.Target = brain.Target.gameObject;
    }
}
