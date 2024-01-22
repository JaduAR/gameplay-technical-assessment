using System.Collections.Generic;
using Game.Assets.Scripts.Utils;
using UnityEngine;

namespace Game.Assets.Scripts.Character.Attacks
{
    [CreateAssetMenu(menuName = "Scriptable Objects/AttackSequence")]
    public class AttackSequence : ScriptableObject
    {
        [SerializeField]
        private List<Attack> _sequence;

        private void OnValidate()
        {
            this.ValidateNotEmpty(_sequence);
        }

        public IEnumerator<Attack> GetAttacks()
        {
            foreach (var attack in _sequence)
            {
                yield return attack;
            }
        }
    }
}
