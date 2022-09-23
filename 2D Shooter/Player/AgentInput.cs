using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentInput : MonoBehaviour, IAgentInput
{
    public static AgentInput instance;

    public Camera mainCamera;
    private bool fireButtonDown = false;
    public bool reloadButtonDown = false;

    [field: SerializeField]
    public UnityEvent<Vector2> OnMovementKeyPressed { get; set; }

    [field: SerializeField]
    public UnityEvent<Vector2> OnPointerPositionChange { get; set; }
    [field: SerializeField]
    public UnityEvent OnFireButtonPressed { get; set; }

    [field: SerializeField]
    public UnityEvent OnFireButtonReleased { get; set; }

    [field: SerializeField]
    public UnityEvent OnReloadButtenPressed { get; set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!GameManager.instance.isPaused && GameManager.instance.canShoot)
        {
            GetMovementInput();
            GetPointerInput();
            GetFireInput();
            GetReloadInput();
        }
    }

    private void GetReloadInput()
    {
        if (Input.GetAxisRaw("Reload") > 0)
        {
            if (reloadButtonDown == false)
            {
                reloadButtonDown = true;
                OnReloadButtenPressed?.Invoke();
            }
        }
        else
        {
            if (reloadButtonDown)
            {
                reloadButtonDown = false;
            }
        }
    }

    private void GetFireInput()
    {
        if (Input.GetAxisRaw("Fire1") > 0 && gameObject != null)
        {
            if (fireButtonDown == false)
            {
                fireButtonDown = true;
                OnFireButtonPressed?.Invoke();
            }
        }
        else
        {
            if (fireButtonDown)
            {
                fireButtonDown = false;
                OnFireButtonReleased?.Invoke();
            }
        }
    }

    private void GetPointerInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCamera.nearClipPlane;
        var mouseInWorldSpace = mainCamera.ScreenToWorldPoint(mousePos);
        OnPointerPositionChange?.Invoke(mouseInWorldSpace);
    }

    private void GetMovementInput()
    {
        OnMovementKeyPressed?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }
}
