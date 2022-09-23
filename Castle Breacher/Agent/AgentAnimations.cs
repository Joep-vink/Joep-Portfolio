using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AgentAnimations : MonoBehaviour
{
    private Animator agentAnimator;
    private Agent agent;

    private void Awake()
    {
        agentAnimator = GetComponent<Animator>();
        agent = GetComponentInParent<Agent>();
    }

    public void AttackAnim()
    {
        agentAnimator.SetTrigger("Attack");
    }

    public void DeathAnim()
    {
        agentAnimator.SetTrigger("Dead");
    }

    public void WalkAnim()
    {
        agentAnimator.SetTrigger("Walk");
    }

    public void AfterDeathAnim()
    {
        GetComponentInParent<Agent>().gameObject.SetActive(false);
    }

    public void StopMoving()
    {
        agent._brain.CanMove = false;
    }

    public void StartMoving()
    {
        agent._brain.CanMove = true;
    }
}

