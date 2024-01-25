using UnityEngine;

public class PlayerHealth : HealthComponent
{
    public delegate void LifeUpdateAction(string tag, int currentHealth);
    public static event LifeUpdateAction OnLifeUpdate;

    public override void ResetHealth()
    {
        base.ResetHealth();
        if(OnLifeUpdate!= null) 
            OnLifeUpdate(gameObject.tag, CurrentHealth);
    }

    public override void Hit(int damageValue)
    {
        base.Hit(damageValue);
        if (CurrentHealth > -1)
        {
            if (OnLifeUpdate != null) 
                OnLifeUpdate(gameObject.tag, CurrentHealth);
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
