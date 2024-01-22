using UnityEngine;
using UnityEngine.UI;

namespace Game.Assets.Scripts.Character.Health.UI
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private GameObject _healthBarStateRoot;

        [SerializeField]
        private GameObject _noTargetStateRoot;

        public enum State
        {
            HealthBar,
            NoTarget
        }

        public void SetHealth(float value)
        {
            _slider.value = value;
        }

        public void SetState(State state)
        {
            switch (state)
            {
                case State.HealthBar:
                    _healthBarStateRoot.SetActive(true);
                    _noTargetStateRoot.SetActive(false);
                    break;
                case State.NoTarget:
                    _healthBarStateRoot.SetActive(false);
                    _noTargetStateRoot.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
