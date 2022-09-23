using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Enemy : Agent, IHittable
{
    public override void Update()
    {
        base.Update();

        LookAt();
    }

    /// <summary>
    /// Look at the defender and if its null look at the point
    /// </summary>
    public override void LookAt()
    {
        base.LookAt();
    }

    public override void GetHit(int damage, GameObject damageDealer)
    {
        base.GetHit(damage, damageDealer);
    }
}
