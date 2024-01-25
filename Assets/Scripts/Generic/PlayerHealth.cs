using UnityEngine;

public class PlayerHealth : HealthComponent
{
    public delegate void LifeUpdateAction(string tag, int currentHealth);
    public static event LifeUpdateAction OnHealtheUpdate;

    public override void ResetHealth()
    {
        base.ResetHealth();
        if(OnHealtheUpdate!= null) 
            OnHealtheUpdate(gameObject.tag, CurrentHealth);
    }

    public override void Hit(int damageValue)
    {
        base.Hit(damageValue);
        if (CurrentHealth > -1)
        {
            //Debug.Log("CurrentHealth : " + CurrentHealth);
            if (OnHealtheUpdate != null) 
                OnHealtheUpdate(gameObject.tag, CurrentHealth);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(gameObject.tag))
        {
            Hit(10);
        }
    }
}
