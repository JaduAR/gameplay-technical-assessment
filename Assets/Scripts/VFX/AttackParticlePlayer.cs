using System.Collections;
using Game.Assets.Scripts.Character.Attacks;
using Game.Assets.Scripts.Utils;
using UnityEngine;

namespace Game.Assets.Scripts.VFX
{
    public class AttackParticlePlayer : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem[] _particleSystems;

        [SerializeField]
        private float _cameraOffset;

        private int _index = 0;

        void OnValidate()
        {
            this.ValidateNotEmpty(_particleSystems);
        }

        public void PlayParticleSystem(Object sender, object data)
        {
            var context = (AttackContext)data;
            StartCoroutine(PlayRandomEffect(context.Target.Position));
        }

        private IEnumerator PlayRandomEffect(Vector3 position)
        {
            if (!_cameraOffset.IsInsignificant())
            {
                position += (Camera.main.transform.position - position).normalized * _cameraOffset;
            }

            var ps = _particleSystems[++_index % _particleSystems.Length];
            ps.transform.position = position;
            ps.gameObject.SetActive(true);
            ps.Play();

            while (ps.isPlaying)
            {
                yield return new WaitForEndOfFrame();
            }

            ps.gameObject.SetActive(false);
        }
    }
}
