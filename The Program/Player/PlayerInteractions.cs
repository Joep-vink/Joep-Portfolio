using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerInteractions : MonoBehaviour
{
    public LayerMask layerToHit;
    private Vector3 h;
    public GameObject interactionInfo;
    private Interactable lastHit;

    private void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 1, layerToHit))
        {
            interactionInfo.gameObject.SetActive(true);
            lastHit = hit.transform.gameObject.GetComponent<Interactable>();

            if (Input.GetKeyDown(KeyCode.E))
                hit.transform.gameObject.GetComponent<IInteractable>().Interact();

            if (Input.GetKeyUp(KeyCode.E))
                hit.transform.gameObject.GetComponent<IInteractable>().StopInteraction();
        }
        else
        {
            if (lastHit != null && lastHit.isActive)
            {
                lastHit.StopInteraction();
                lastHit = null;
            }

            interactionInfo.gameObject.SetActive(false);
        }

        h = hit.point;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, h);
    }
}
