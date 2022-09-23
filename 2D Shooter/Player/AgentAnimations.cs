using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AgentAnimations : MonoBehaviour
{
    public Animator agentAnimator;

    public FeedbackPlayer feedback;

    private void Awake()
    {
        agentAnimator = GetComponent<Animator>();
    }

    public void SetWalkAnimation(bool val)
    {
        agentAnimator.SetBool("Walk", val);
    }

    public void AnimatePlayer(float velocity)
    {
        SetWalkAnimation(velocity > 0);
    }

    public void PlayDeathAnimation()
    {
        agentAnimator.SetTrigger("Death");
    }

    public void SetWalkSpeed(int val)
    {
        agentAnimator.SetFloat("WalkMultiplier", val);
    }

    public void PlayFeedback()
    {
        feedback.PlayFeedback();
    }
}
