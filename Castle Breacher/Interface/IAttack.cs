using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    void AttackOnStart(AIBrain brain, AgentSO agentSo, AgentAnimations anim, ObjectPool pool);
    void Attack(AIBrain brain, AgentSO agentSo, AgentAnimations anim, ObjectPool pool);
}
