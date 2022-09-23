using UnityEngine;

public class ChaseAction : AIAction
{
    public override void TakeAction()
    {
        if (aiBrain.Target == null) return; //Check if there is a target

        if (aiBrain.Target.gameObject.activeInHierarchy)
        {
            Vector3 dir = aiBrain.Target.position - transform.position; //The distance between the position and the target
            aiBrain.transform.Translate(dir.normalized * aiBrain.agentSO.speed * Time.deltaTime); //Move the agent
        }
    }
}
