using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHittable
{
    UnityEvent OnGetHit { get; set; }

    //UnityEvent OnDie { get; set; }

    /// <summary>
    /// Deal damage and call the OnGetHit event
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageDealer"></param>
    void GetHit(int damage, GameObject damageDealer);
}
