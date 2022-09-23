using UnityEngine;

public class DefMoveAction : AIAction
{
    public override void TakeAction()
    {
        Vector3 dir = aiBrain.point - transform.position; //The distance between the position and the target
        aiBrain.gameObject.transform.Translate(dir.normalized * aiBrain.agentSO.speed * Time.deltaTime); //Move the agent

        if (Vector3.Distance(transform.position, aiBrain.point) <= 0.2f) //If the agent is in distance
        {
            if (aiBrain.wavePointIndex <= 0)
            {
                var ag = GetComponentInParent<Agent>();
                ag.OnDie?.Invoke();

                UiManager.instanace.UpdateHealth(1, true);
                ag.IsDead = true;
                aiBrain.Target = null;

                return; //Check if agent is already at the end
            }

            aiBrain.wavePointIndex--;

            aiBrain.CalculateNextPos(aiBrain.wavePointIndex);
        }
    }
}
