using UnityEngine;

public class AttackDetectionBehavior : MonoBehaviour
{
    [SerializeField] private bool _showGizmo;
    [SerializeField] private float _distanceToAttack;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _opponent;

    private bool _isInRangeToAttack = false;
    public bool IsInRangeToAttack => _isInRangeToAttack;
    
    private void FixedUpdate()
    {
        _isInRangeToAttack = Vector3.Distance(_player.position, _opponent.position) <= _distanceToAttack;
    }

    private void OnDrawGizmos()
    {
        if (!_showGizmo)
            return;
        
        if(IsInRangeToAttack)
        {
            Debug.DrawLine(_player.position, _opponent.position, Color.red);   
        }else
        {
            Debug.DrawLine(_player.position, _opponent.position, Color.white);
        }
    }
}
