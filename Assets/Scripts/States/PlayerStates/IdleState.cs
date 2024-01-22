using UnityEngine;

// This class represents the Idle state of the player
public class IdleState : IState
{
	private Player player;
    private Animator animator;

	// Constructor to Initialize the player and animator variables
    public IdleState(Player player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }

	// Runs when we first enter the state
	public void Enter()
	{
		// Set the StrafeX and StrafeZ parameters to 0 to stop the player from moving
		animator.SetFloat("StrafeX", 0);
        animator.SetFloat("StrafeZ", 0);
	}

	public void Update()
	{
		// Get the input from the player
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

		// If the movement is greater than 0.1, change the state to the MoveState
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            player.ChangeState(player.moveState);
        }
	}

	// Runs when we exit the state
	public void Exit()
	{
		
	}

	// Runs when the user presses the punch button
	public void HandleInput()
	{
		// If the player presses the punch button, 
		// change the state to the Punch1State
		player.ChangeState(player.punch1State);
	}
}
