using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Transform[] points;

    private void Awake()
    {
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++) //Add all the clidren to the list
        {
            points[i] = transform.GetChild(i); 
        }
    }

    private void OnDrawGizmos()
    {
        //Draw all the circles and a line between them
        for (int i = 0; i < points.Length; i++)
        {
            if (i < points.Length - 1)
                Gizmos.DrawLine(points[i].position, points[i + 1].position);

            Gizmos.DrawWireSphere(points[i].position, GameManager.instance.followPathRadius);
        }
    }
}
