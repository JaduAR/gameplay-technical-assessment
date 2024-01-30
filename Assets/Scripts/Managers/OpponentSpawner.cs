using Game.Assets.Scripts.EventSystem;
using Game.Assets.Scripts.Utils;
using UnityEngine;

namespace Game.Assets.Scripts.Managers
{
    public class OpponentSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _opponentPrefab;

        [SerializeField] 
        private GameObject[] _spawnPoints;

        [SerializeField]
        private GameEvent _spawnEvent;

        [SerializeField]
        private GameEvent _deathEvent;

        private void OnValidate()
        {
            this.ValidateReference(_opponentPrefab);
            this.ValidateNotEmpty(_spawnPoints);
        }

        private void OnEnable()
        {
            _deathEvent.EventRaised += OnDeath;
        }

        private void OnDisable()
        {
            _deathEvent.EventRaised -= OnDeath;
        }

        private void Start()
        {
            SpawnOpponent();
        }

        private void OnDeath(Object sender, object data)
        {
            var senderComponent = (Component) sender;
            Destroy(senderComponent.gameObject);
            SpawnOpponent();
        }

        private void SpawnOpponent()
        {
            var go = Instantiate(_opponentPrefab, FindSpawnPosition(), Quaternion.identity);
            _spawnEvent?.Raise(this, go);
        }

        private Vector3 FindSpawnPosition()
        {
            return _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform.position;
        }
    }
}
