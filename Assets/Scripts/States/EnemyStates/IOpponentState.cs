public interface IOpponentState
{
	// Runs when we first enter the state
	void Enter();

	// Runs every frame, and include condition to transition to other states
	void Update();

	// Runs when we exit the state
	void Exit();
}
