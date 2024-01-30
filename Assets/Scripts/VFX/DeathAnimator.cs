using System.Collections;
using Game.Assets.Scripts.Utils;
using UnityEngine;

namespace Game.Assets.Scripts.VFX
{
    public class DeathAnimator : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _deathParticle;

        void OnValidate()
        {
            this.ValidateReference(_deathParticle);
        }

        public void OnDeath(Object sender, object data)
        {
            var go = ((Component)sender).gameObject;
            StartCoroutine(DeathCoroutine(go));
        }

        private IEnumerator DeathCoroutine(GameObject target)
        {
            _deathParticle.transform.position = target.transform.position;
            _deathParticle.gameObject.SetActive(true);
            _deathParticle.Play();

            while (_deathParticle.isPlaying)
            {
                yield return new WaitForEndOfFrame();
            }

            _deathParticle.gameObject.SetActive(false);
        }
    }
}
