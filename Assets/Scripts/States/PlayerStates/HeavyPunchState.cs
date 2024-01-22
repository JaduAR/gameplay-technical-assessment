using UnityEngine;

// This class represents the third or final punch in the combo
public class HeavyPunchState : IState
{
    private Player player;
    private Animator animator;

	// Constructor to Initialize the player and animator variables
    public HeavyPunchState(Player player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }

	// Runs when we first enter the state
    public void Enter()
    {
		// Set the trigger to play the heavy punch animation
        animator.SetTrigger("HeavyPunch");

		// Set the last punch time to the current time
        player.LastPunchTime = Time.time;
    }

    public void Update()
    {
		// Transition to the IdleState after 2.25 seconds, so the player can 
		// move or punch again
        if (Time.time - player.LastPunchTime > 2.25f)
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

	}	
}
