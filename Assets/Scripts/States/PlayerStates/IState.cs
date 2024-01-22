public interface IState
{
	// Runs when we first enter the state
	void Enter();

	// Runs every frame, and include condition to transition to other states
	void Update();

	// Runs when we exit the state
	void Exit();

	// Runs when the user presses the punch button on the UI
	void HandleInput();
}
