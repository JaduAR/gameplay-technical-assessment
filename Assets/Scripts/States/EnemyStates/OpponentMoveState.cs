using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentMoveState : IOpponentState
{
    private Opponent opponent;
    private Animator animator;
    private readonly Player player;
	private readonly float stopMoveDistance; // The distance at which the opponent stops moving towards the player

	// Constructor to initialize the opponent, animator, and player variables
    public OpponentMoveState(Opponent opponent, Animator animator, Player player, float stopMoveDistance)
    {
        this.opponent = opponent;
        this.animator = animator;
        this.player = player;
		this.stopMoveDistance = stopMoveDistance;
    }

	// Runs when we first enter the state
    public void Enter() 
	{ 

	}

	// Runs every frame
    public void Update()
    {
		// Calculate the direction to the player
		Vector3 direction = (player.transform.position - opponent.transform.position).normalized;

		// Calculate the local direction relative to the opponent's rotation
		Vector3 localDirection = opponent.transform.InverseTransformDirection(direction);

		// Set the StrafeX and StrafeZ parameters based on the local direction to the player
		animator.SetFloat("StrafeX", localDirection.x);
		animator.SetFloat("StrafeZ", localDirection.z);

        // If the player is close enough, stop moving
        if (Vector3.Distance(opponent.transform.position, player.transform.position) <= stopMoveDistance)
        {
            opponent.ChangeState(opponent.idleState);
        }
    }

	// Runs when we exit the state
    public void Exit() 
	{ 

	}
}
