using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Fighter : MonoBehaviour
{
    [SerializeField]
    public FighterState CurrentFighterState;

    [Header("Hit Counter")]
    [SerializeField]
    public int HitCounter = 3;

    private void OnEnable()
    {
        TapController.OnTap += Punch;
    }

    private void OnDisable()
    {
        TapController.OnTap -= Punch;
    }

    private void OnDestroy()
    {
        TapController.OnTap -= Punch;
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentFighterState = new IdleFS();
        CurrentFighterState.Execute(this);
    }

    public void LogStatus(string status)
    {
        Debug.Log("Current State: " + CurrentFighterState.ID);
    }

    public void SetState(FighterState state)
    {
        CurrentFighterState = state;
        state.Execute(this);
    }

    private void Punch(string name, float diff)
    {
        if (name != "AttackButtonTap") return;

        if (diff <= 0.5f)
        {
            if (HitCounter >= 2)
            {
                SetState(new AttackChargingFS());
                //ResetHitCounter(); // reset counter
            }
            else
            {
                SetState(new AttackP2FS());
            }
        }
        else
        {
            //ResetHitCounter();
            SetState(new AttackP1FS());
        }
    }

    /// <summary>
    /// Set animator parameter (trigger)
    /// </summary>
    /// <param name="parameterName"></param>
    public void SetAnim(string parameterName)
    {
        Animator _animator = GetComponent<Animator>();
        if (!_animator) return;
        
        _animator.SetTrigger(parameterName);
    }

    /// <summary>
    /// Set animator parameter (float)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="parameterName"></param>
    public void SetAnim(string parameterName, float value)
    {
        Animator _animator = GetComponent<Animator>();
        if (!_animator) return;

        _animator.SetFloat(parameterName,value);
    }

    /// <summary>
    /// When an attack is landed, the HitCounter increases 1.
    /// </summary>
    private void Hit()
    {
        HitCounter++;
    }

    private void ResetHitCounter()
    {
        HitCounter = 0;
    }
}
