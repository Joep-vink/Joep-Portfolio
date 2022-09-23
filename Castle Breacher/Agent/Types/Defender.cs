using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Defender : Agent, IHittable
{
    public override void Update()
    {
        base.Update();

        LookAt();
    }

    public override void LookAt()
    {
        base.LookAt();
    }

    public override void GetHit(int damage, GameObject damageDealer)
    {
        base.GetHit(damage, damageDealer);
    }
}
