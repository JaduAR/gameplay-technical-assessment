using UnityEngine;

public class OpponentIdleState : IOpponentState
{
    private Opponent opponent;
    private Animator animator;
    private readonly Player player;
	private readonly float startMoveDistance; // The distance at which the opponent starts moving towards the player

	// Constructor to initialize the opponent, animator, and player variables
    public OpponentIdleState(Opponent opponent, Animator animator, Player player, float startMoveDistance)
    {
        this.opponent = opponent;
        this.animator = animator;
        this.player = player;
		this.startMoveDistance = startMoveDistance;

    }

	// Runs when we first enter the state
    public void Enter() 
	{ 
		// Set the StrafeX and StrafeZ parameters to 0, so the opponent stops moving
		animator.SetFloat("StrafeX", 0);
		animator.SetFloat("StrafeZ", 0);
	}

	// Runs every frame
    public void Update()
    {
        // If the player is far enough away, start moving towards them
        if (Vector3.Distance(opponent.transform.position, player.transform.position) > startMoveDistance)
        {
            opponent.ChangeState(opponent.moveState);
        }
    }

	// Runs when we exit the state
    public void Exit() 
	{ 

	}
}
