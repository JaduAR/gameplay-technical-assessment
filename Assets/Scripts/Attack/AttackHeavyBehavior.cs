using System.Collections;
using UnityEngine;

public class AttackHeavyBehavior : MonoBehaviour
{
    [SerializeField] private AttackAnimationsBehavior _animationsBehavior;
    [SerializeField] private float _comboTimeRange = 0.5f;
    [SerializeField] private int _punchAmounToHeavy = 2;
    
    private float _comboTime = 0f;
    private int _punchCounter = 0;
    private bool _inCombo = false;
    private bool _heavyTriggered = false;

    private void Update()
    {
        _comboTime -= Time.deltaTime;
        
        if (_heavyTriggered)
            return;

        if (_comboTime < 0f)
        {
            _comboTime = 0f;
            _punchCounter = 0;
            
            _animationsBehavior.ChangeHeavyPunch(false);
        }

        _inCombo = _comboTime > 0f;

        if (_inCombo && _punchCounter >= _punchAmounToHeavy)
        {
            _heavyTriggered = true;
            StartCoroutine(HeavyPunchCo());
        }
    }

    private IEnumerator HeavyPunchCo()
    {
        _animationsBehavior.ChangeHeavyPunch(true);
        while (_comboTime > 0f)
        {
            yield return null;
        }
        // ResetCombo();
    }

    private void ResetCombo()
    {
        _animationsBehavior.ChangeHeavyPunch(false);
        _comboTime = 0f;
        _punchCounter = 0;
        _inCombo = false;
        
        _heavyTriggered = false;
    }

    public void ReceivePunch()
    {
        _comboTime = _comboTimeRange;
        _punchCounter += 1;
    }

    public void ReceiveHeavyPunch()
    {
        ResetCombo();
    }
}
