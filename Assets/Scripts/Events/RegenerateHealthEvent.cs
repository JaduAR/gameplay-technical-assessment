using UnityEngine.Events;

public class RegenerateHealthEvent : UnityEvent<int>
{
	public static RegenerateHealthEvent Instance = new RegenerateHealthEvent();
}
