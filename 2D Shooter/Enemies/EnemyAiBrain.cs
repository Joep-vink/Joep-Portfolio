using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAiBrain : MonoBehaviour, IAgentInput
{
    [field: SerializeField]
    public GameObject Target { get; set; }

    [field: SerializeField]
    public AIState currentState { get; private set; }

    [field: SerializeField]
    public UnityEvent OnFireButtonPressed { get; set; }

    [field: SerializeField]
    public UnityEvent OnFireButtonReleased { get; set; }

    [field: SerializeField]
    public UnityEvent<Vector2> OnMovementKeyPressed { get; set; }

    [field: SerializeField]
    public UnityEvent<Vector2> OnPointerPositionChange { get; set; }

    private void Awake()
    {
        Target = FindObjectOfType<player>().gameObject;
    }

    private void Update()
    {
        if (Target == null)
        {
            OnMovementKeyPressed?.Invoke(Vector2.zero);
        }
        else
        {
            currentState.UpdateState();
        }
    }

    public void Attack()
    {
        OnFireButtonPressed?.Invoke();
    }

    public void Move(Vector2 moveDirection, Vector2 targetPosition)
    {
        OnMovementKeyPressed?.Invoke(moveDirection);
        OnPointerPositionChange?.Invoke(targetPosition);
    }

    internal void ChangeToState(AIState state)
    {
        currentState = state;
    }
}
