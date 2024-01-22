using UnityEngine;

// Represents the main player character in the scene. 
public class Player : MonoBehaviour
{
	[SerializeField] private Transform opponent;
	[SerializeField] private float rotationSpeed = 5f; // How fast the player rotates to face the opponent
	[SerializeField] private float attackRange = 0.5f; // The range at which the player can attack the opponent

    public IState CurrentState { get; private set; } // The current state of the player
    public IdleState idleState;
    public MoveState moveState;	
    public Punch1State punch1State;
    public Punch2State punch2State;
    public HeavyPunchState heavyPunchState;

    private Animator animator;
    private float lastPunchTime;
	public float LastPunchTime
	{
		get { return lastPunchTime; }
		set { lastPunchTime = value; }
	}

    // Start is called before the first frame update
    void Start()
    {
		// Get the attached animator component
        animator = GetComponent<Animator>();

		// Initialize the states
        idleState = new IdleState(this, animator);
        moveState = new MoveState(this, animator);
        punch1State = new Punch1State(this, animator);
        punch2State = new Punch2State(this, animator);
        heavyPunchState = new HeavyPunchState(this, animator);

		// Set the starting state to the IdleState
        CurrentState = idleState;
		// Enter the starting state
        CurrentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
		// Update the current state
        CurrentState.Update();

		// Rotate to face the opponent
		Vector3 direction = (opponent.transform.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

	/// <summary>
	/// Changes the current state to the newState
	/// </summary>
	public void ChangeState(IState newState)
    {
		// Exit the current state
        CurrentState.Exit();
		// Set the current state to the newState
        CurrentState = newState;
		// Enter the new state
        CurrentState.Enter();
    }

	/// <summary>
	/// Deals damage to the opponent if they are within the attack range.
	/// Called from animation events and give the damage according to the 
	/// animation that is playing. Punch1 and Punch2 deal 10 damage,
	/// HeavyPunch deals 100 damage.
	/// </summary>	
	public void DealDamage(int damage)
	{
		// Check the distance between the player and the opponent
		float distance = Vector3.Distance(transform.position, opponent.transform.position);

		// If the opponent is within the attach range
		if (distance <= attackRange)
		{
			// Deal damage to the opponent
			opponent.GetComponent<Health>().TakeDamage(damage);
		}
	}	
}
