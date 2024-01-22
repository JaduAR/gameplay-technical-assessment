using UnityEngine;

// This class represents the second punch in the combo
public class Punch2State : IState
{
    private Player player;
    private Animator animator;

	// Constructor to Initialize the player and animator variables
    public Punch2State(Player player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }	

	// Runs when we first enter the state
	public void Enter()
	{
		// Set the trigger to play the punch 2 animation
        animator.SetTrigger("Punch2");

		// Set the last punch time to the current time
        player.LastPunchTime = Time.time;
	}

	public void Update()
	{
		// If the time since the last punch is greater than 0.5 seconds,
		// change the state back to the IdleState
		if (Time.time >= player.LastPunchTime + 0.5f)
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
		// If the player presses the punch button third time within 0.5 seconds,
		// transition to heavy or final punch state.
		player.ChangeState(player.heavyPunchState);
	}	
}
