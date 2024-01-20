using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;


    private const string _kPunchOneParameter = "P1";
    private const string _kPunchTwoParameter = "P2";
    private const string _kPunchChargeParameter = "Charge";

    private float lastClickTime = 0f;
    private int consecutiveClicks = 0;
    private float doubleClickThreshold = 0.3f;
    private float tripleClickThreshold = 0.23f;
    private float _resetDelay = 0.5f;

    private EPunchType _punchType = EPunchType.none;
    public EPunchType PunchType => _punchType;

 
    public void OnUIButtonClick()
    {
       
        float currentTime = Time.time;
        PunchOne();
        // Check for double-click
        if (currentTime - lastClickTime < doubleClickThreshold)
        {
            consecutiveClicks++;

            PunchTwo();
            // Check for triple-click
            if (consecutiveClicks == 3 && currentTime - lastClickTime < tripleClickThreshold)
            {
                Debug.Log("Triple click detected!");
                // Your logic for handling triple click goes here
                consecutiveClicks = 0; // Reset the counter
                PunchCharge();
            }
        }
        else
        {
            consecutiveClicks = 1; // Reset the counter for the first click of a new sequence
        }

        lastClickTime = currentTime;
    }


    private void PunchOne()
    {
        Debug.Log("Punch 1");
        _animator.SetBool(_kPunchOneParameter, true);
        _animator.SetBool(_kPunchTwoParameter, false);
        _animator.SetBool(_kPunchChargeParameter, false);
        Invoke("ResetAll", _resetDelay);
        _punchType = EPunchType.punch1;
    }

    private void PunchTwo()
    {
        Debug.Log("Punch 2");

        _animator.SetBool(_kPunchOneParameter, true);
        _animator.SetBool(_kPunchTwoParameter, true);
        _animator.SetBool(_kPunchChargeParameter, false);
        CancelInvoke("ResetAll");
        Invoke("ResetAll", _resetDelay);
        _punchType = EPunchType.punch2;

    }
    private void PunchCharge()
    {
        Debug.Log("Punch Charge");

        _animator.SetBool(_kPunchOneParameter, true);
        _animator.SetBool(_kPunchTwoParameter, true);
        _animator.SetBool(_kPunchChargeParameter, true);
        CancelInvoke("ResetAll");
        Invoke("ResetAll", _resetDelay);
        _punchType = EPunchType.charged;

    }

    public void ResetAll()
    {
        Debug.Log("Reset all");

        _animator.SetBool(_kPunchOneParameter, false);
        _animator.SetBool(_kPunchTwoParameter, false);
        _animator.SetBool(_kPunchChargeParameter, false);
        _punchType = EPunchType.none;
    }
}