using UnityEngine;

// This class represents the state when the player is moving
public class MoveState : IState
{
	private Player player;
    private Animator animator;

	// Constructor to Initialize the player and animator variables
    public MoveState(Player player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }

	// Runs when we first enter the state
    public void Enter()
    {
        
    }

    public void Update()
    {
		// Get the input from the player
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = -Input.GetAxis("Vertical");

		// Set the StrafeX and StrafeZ parameters to the input from the player
        animator.SetFloat("StrafeX", vertical);
        animator.SetFloat("StrafeZ", horizontal);

		// If the movement is less than 0.1, set the state to the IdleState
        if (Mathf.Abs(horizontal) < 0.1f && Mathf.Abs(vertical) < 0.1f)
        {
            player.ChangeState(player.idleState);
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
