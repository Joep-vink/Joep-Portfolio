using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Computer : MonoBehaviour, IInteractable
{
    [field: SerializeField] public UnityEvent OnInteract { get; set; }

    [field: SerializeField] public UnityEvent OnLeave { get; set; }

    public bool isActive { get; set; }
    public float sound = 1;

    public void Interact()
    {
        if (!isActive)
        {
            OnInteract?.Invoke();
            
            StartCoroutine(NoiseManager.instance.AddValue(5, 0.5f));
            NoiseManager.instance.currMultiplier += sound;

            AudioManager.instance.Play("Typing");
            AudioManager.instance.Play("Computer");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            NoiseManager.instance.StopAddingOverTime(1);

            AudioManager.instance.Stop("Typing");
            AudioManager.instance.Stop("Computer");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            OnLeave?.Invoke();
        }
        isActive = !isActive;
    }

    public void StopInteraction()
    {

    }
}
