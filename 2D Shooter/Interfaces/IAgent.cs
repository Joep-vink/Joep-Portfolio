using UnityEngine;
using UnityEngine.Events;

public interface IAgent
{
    float Health { get; }
    UnityEvent OnDie { get; set; }
    UnityEvent OnGetHit { get; set; }
}