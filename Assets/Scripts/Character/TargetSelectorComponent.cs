using System.Collections;
using System.Linq;
using Game.Assets.Scripts.EventSystem;
using Game.Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Assets.Scripts.Character
{
    public interface ITargetSelector
    {
        [CanBeNull] ICharacter Target { get; }
    }

    public class TargetSelectorComponent : MonoBehaviour, ITargetSelector
    {
        [SerializeField]
        private Character _target;

        [SerializeField]
        private float _range = 5f;

        [SerializeField]
        private GameEvent _targetChangeEvent;

        [SerializeField] 
        private GameEvent _deathEvent;

        [SerializeField]
        private GameEvent _spawnEvent;

        public ICharacter Target => _target;

        private Coroutine _targetSearchCoroutine;

        void OnValidate()
        {
            this.ValidateReference(_targetChangeEvent);
            this.ValidateReference(_deathEvent);
            this.ValidateReference(_spawnEvent);
        }

        void OnEnable()
        {
            _deathEvent.EventRaised += OnDeathEventRaised;
            _spawnEvent.EventRaised += OnSpawnEventRaised;

            SetTarget(_target);
            _targetSearchCoroutine = StartCoroutine(TargetSearchCoroutine());
        }

        void OnDisable()
        {
            _deathEvent.EventRaised -= OnDeathEventRaised;
            _spawnEvent.EventRaised -= OnSpawnEventRaised;

            StopCoroutine(_targetSearchCoroutine);
        }

        private void OnSpawnEventRaised(Object sender, object data)
        {
            SetTarget(data as Character);
        }

        private void OnDeathEventRaised(Object sender, object data)
        {
            SetTarget(null);
        }

        private void SetTarget(Character target)
        {
            _target = target;
            _targetChangeEvent.Raise(this, target);
        }

        private IEnumerator TargetSearchCoroutine()
        {
            const float refreshRate = 0.3f;

            while (true)
            {
                if (_target == null)
                {
                    var closestTarget = FindClosestEnemy();
                    if (closestTarget != null && IsInRange(closestTarget))
                    {
                        SetTarget(closestTarget);
                    }
                }
                else
                {
                    if (!IsInRange(_target))
                    {
                        SetTarget(null);
                    }
                }

                yield return new WaitForSeconds(refreshRate);
            }
        }

        private Character FindClosestEnemy()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy").Where(e => e.activeSelf).ToArray();

            if (!enemies.Any())
                return null;

            return enemies[Random.Range(0, enemies.Length)].GetComponent<Character>();
        }

        private bool IsInRange(Character target)
        {
            return Vector3.Distance(transform.position, target.Position) <= _range;
        }
    }
}