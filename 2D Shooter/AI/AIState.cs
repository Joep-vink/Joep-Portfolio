using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : MonoBehaviour
{
    private EnemyAiBrain enemyBrain = null;
    [SerializeField] private List<AIAction> actions = null;
    [SerializeField] private List<AITransition> transitions = null;

    private void Awake()
    {
        enemyBrain = transform.root.GetComponent<EnemyAiBrain>();
    }

    public void UpdateState()
    {
        foreach (var action in actions)
        {
            action.TakeAction();
        }
        foreach (var transition in transitions)
        {
            bool result = false;
            foreach (var desicion in transition.Decisions)
            {
                result = desicion.MakeADecision();
                if (!result)
                {
                    break;
                }
            }
            if (result)
            {
                if (transition.PositiveResult != null)
                {
                    enemyBrain.ChangeToState(transition.PositiveResult);
                    return;
                }
            }
            else
            {
                if (transition.NegativeResult != null)
                {
                    enemyBrain.ChangeToState(transition.NegativeResult);
                }
            }
        }
    }
}
