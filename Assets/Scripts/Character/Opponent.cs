using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    [SerializeField] private Player player; // Reference to the player
	[SerializeField] private float rotationSpeed = 5f; // How fast the opponent rotates to face the player
	[SerializeField] private float startMoveDistance = 2.5f; // The distance at which the opponent starts moving towards the player
	[SerializeField] private float stopMoveDistance = 1.5f; // The distance at which the opponent stops moving towards the player

    private Animator animator;

    public IOpponentState idleState;
    public IOpponentState moveState;
    private IOpponentState currentState;

    private void Start()
    {
		// Get the attached animator component
		animator = GetComponent<Animator>();

		// Initialize the states
        idleState = new OpponentIdleState(this, animator, player, startMoveDistance);
        moveState = new OpponentMoveState(this, animator, player, stopMoveDistance);

		// Set the current state to the IdleState
        currentState = idleState;
    }

    private void Update()
    {
        // Rotate to face the player
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

		// Update the current state
        currentState.Update();
    }

	/// <summary>
	/// Changes the current state to the newState
	/// </summary>
    public void ChangeState(IOpponentState newState)
    {
        currentState = newState;
        currentState.Enter();
    }
}
