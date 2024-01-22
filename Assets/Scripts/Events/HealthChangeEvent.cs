using UnityEngine.Events;

/// <summary>
/// This class represents an event that is triggered when the health of an object changes.
/// Used to update the health bar when the health of an object changes.
/// </summary>
public class HealthChangeEvent : UnityEvent<int>
{
	public static HealthChangeEvent Instance = new HealthChangeEvent();
}
