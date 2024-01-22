using UnityEngine;

namespace Game.Assets.Scripts.Character
{
    [RequireComponent(typeof(Character))]
    public class AimComponent : MonoBehaviour
    {
        private ITargetSelector _targetProvider;

        private void Awake()
        {
            _targetProvider = GetComponent<ITargetSelector>();
        }

        private void Update()
        {
            if (_targetProvider?.Target != null)
            {
                var rotation = Quaternion.LookRotation(_targetProvider.Target.Position - transform.position, Vector3.up);

                transform.rotation = Quaternion.AngleAxis(rotation.eulerAngles.y, Vector3.up);
            }
        }
    }
}
