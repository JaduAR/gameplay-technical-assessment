using UnityEngine;

// This class represents the first punch in the combo
public class Punch1State : IState
{
    private Player player;
    private Animator animator;

	// Constructor to Initialize the player and animator variables
    public Punch1State(Player player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }

	// Runs when we first enter the state
    public void Enter()
    {
		// Set the trigger to play the punch 1 animation
        animator.SetTrigger("Punch1");

		// Set the last punch time to the current time
        player.LastPunchTime = Time.time;
    }

    public void Update()
    {
		// If the time since the last punch is greater than 0.5 seconds,
		// change the state back to the IdleState
        if (Time.time - player.LastPunchTime > 0.5f)
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
		// If the player presses the punch button again within 0.5 seconds,
		// transition to the next state
		player.ChangeState(player.punch2State);
	}	
}
