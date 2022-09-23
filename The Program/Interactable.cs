using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [field: SerializeField] public UnityEvent OnInteract { get; set; }
    [field: SerializeField] public UnityEvent OnLeave { get; set; }

    public bool isActive { get; set; }

    public virtual void Interact()
    {
        if (!isActive)
            OnInteract?.Invoke();
        else
            OnLeave?.Invoke();

        isActive = !isActive;
    }

    public virtual void StopInteraction()
    {

    }
}
