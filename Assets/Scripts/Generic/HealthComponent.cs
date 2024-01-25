using UnityEngine;

    public class HealthComponent : MonoBehaviour, IDamageable
    {
        public delegate void DeathAction(string tag);
        public static event DeathAction OnDeath;

        [SerializeField]
        protected int currentHealth = 100;
        [HideInInspector]
        public virtual int CurrentHealth
        {
            get { return currentHealth; }
            private set { currentHealth = value; }
        }

        [SerializeField]
        protected int maxHealth = 100;
        [HideInInspector]
        public virtual int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        protected virtual void OnEnable()
        {
            ResetHealth();
        }

        /// <summary>
        /// Reset Health to max health
        /// </summary>
        public virtual void ResetHealth()
        {
            CurrentHealth = MaxHealth;
        }

        /// <summary>
        /// Reduce current health
        /// </summary>
        public virtual void Hit(int damageValue)
        {
            if (CurrentHealth > 0)
            {
                CurrentHealth -= damageValue;
                
                if (CurrentHealth < 1)
                {
                    if(OnDeath!= null)
                        OnDeath(gameObject.tag);

                    Debug.Log(gameObject.tag + " is Dead. ");
                }
            }
        }

    }
