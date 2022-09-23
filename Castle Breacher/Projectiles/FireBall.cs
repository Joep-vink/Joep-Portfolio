using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectile
{
    protected override void MoveToTarget()
    {
        base.MoveToTarget();

        Vector3 dir = Target.transform.position - transform.position; //The distance between the position and the target
        rb.MovePosition(transform.position + (dir.normalized * ProjectileSO.speed * Time.deltaTime)); //Move the object

        transform.rotation = LookAtTarget(Target.transform.position - transform.position);
    }
}
