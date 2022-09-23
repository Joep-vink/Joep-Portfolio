using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    public UnityEvent OnInteract { get; set; }
    public UnityEvent OnLeave { get; set; }

    public bool isActive { get; set; }

    public void Interact();

    public void StopInteraction();
}
