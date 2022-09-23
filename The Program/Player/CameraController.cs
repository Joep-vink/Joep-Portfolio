using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Texture2D cursor;
    public Vector3 hotSpotPos;

    [Header("Private")]
    private PlayerController body;

    private void Start()
    {
        body = GetComponentInParent<PlayerController>();

        Cursor.SetCursor(cursor, hotSpotPos, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        body.transform.localRotation = Quaternion.Euler(Quaternion.identity.x, transform.eulerAngles.y, Quaternion.identity.z);
    }
}
