using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LookDecision : AIDecision
{
    [Range(1, 15)]
    [SerializeField] private float distance = 15;
    [SerializeField] private LayerMask rayCastMask = new LayerMask();

    [field: SerializeField]
    public UnityEvent  OnPlayerSpotted { get; set; }

    public override bool MakeADecision()
    {
        var direction = enemyAiBrain.Target.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, rayCastMask);
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPlayerSpotted?.Invoke();
            return true;
        }
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject && enemyAiBrain != null && enemyAiBrain.Target != null)
        {
            Gizmos.color = Color.red;
            var direction = enemyAiBrain.Target.transform.position - transform.position;
            Gizmos.DrawRay(transform.position, direction.normalized * distance);
        }
    }
#endif
}
