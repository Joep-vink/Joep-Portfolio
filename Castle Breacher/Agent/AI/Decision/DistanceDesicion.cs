using UnityEngine;

public class DistanceDesicion : AIDecision
{
    [field: SerializeField]
    [field: Range(0.1f, 10)]
    public float Distance { get; set; } = 5;

    /// <summary>
    /// Check if the enemy is active and if your in range
    /// </summary>
    /// <returns></returns>
    public override bool MakeADecision()
    {
        if (enemyBrain.Target == null || !enemyBrain.Target.gameObject.activeInHierarchy) return false; //Check if target is active

        if (Vector3.Distance(enemyBrain.Target.transform.position, transform.position) < Distance) //Check if target is in range
        {
            if (aIActionData.TargetSpotted == false)
            {
                aIActionData.TargetSpotted = true;
            }   
        }
        else
        {
            aIActionData.TargetSpotted = false;
        }
        return aIActionData.TargetSpotted; //Retunrn treu or false
    }

#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Distance);
            Gizmos.color = Color.white;
        }
    }
#endif
}
