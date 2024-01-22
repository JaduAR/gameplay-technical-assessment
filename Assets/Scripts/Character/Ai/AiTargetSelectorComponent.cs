using UnityEngine;

namespace Game.Assets.Scripts.Character.Ai
{
    public class AiTargetSelectorComponent : MonoBehaviour, ITargetSelector
    {
        public ICharacter Target { get; private set; }

        private void Update()
        {
            Target ??= GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        }
    }
}
