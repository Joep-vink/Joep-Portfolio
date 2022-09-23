using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AIBrain : MonoBehaviour
{
    public AgentSO agentSO;
    [field: SerializeField]
    public Transform Target { get; set; }     

    [field: SerializeField]
    public AIState currentState { get; private set; }

    [field: SerializeField]
    public UnityEvent OnAttack { get; set; }

    [Header("Path")]
    public int wavePointIndex = 0;
    [HideInInspector] public Vector3 point;
    [HideInInspector] public bool CanMove = true;

    private void Start()
    {
        CalculateNextPos(wavePointIndex);
    }

    private void Update()
    {
        if (!GetComponent<Agent>().IsDead)
        currentState.UpdateState();
    }

    /// <summary>
    /// Get a random position in the radius to move to
    /// </summary>
    /// <param name="index">the next point your moving to</param>
    public void CalculateNextPos(int index)
    {
        var game = GameManager.instance;
        var randX = Random.Range(game.path.points[index].position.x + game.followPathRadius, game.path.points[index].position.x - game.followPathRadius);
        var randY = Random.Range(game.path.points[index].position.y + game.followPathRadius, game.path.points[index].position.y - game.followPathRadius);

        var newPos = new Vector3(randX, randY, 0); //the new pos

        point = newPos; //Set the new position
    }

    /// <summary>
    /// Invoke the OnAttack
    /// </summary>
    public void Attack()
    {
        OnAttack?.Invoke();
    }

    /// <summary>
    /// Set the current state to the given state
    /// </summary>
    /// <param name="state">the state your changing to</param>
    internal void ChangeToState(AIState state)
    {
        currentState = state;
    }
}
