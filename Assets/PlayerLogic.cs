using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLogic : MonoBehaviour
    {
    public UnityEvent<int> onDoDamage;
    public Transform opponent;
    public float closeDistance;

    PlayerInputs inputs;
    Animator animator;

    public const float COMBO_TIMER = 1.0f; 

    public float moveAccel;

    int landedPunches;
    int numPresses;
    bool isHeavyPunchReady;
    float timer = 0;
    bool isPunching = false;
    bool isHeavyPunching = false;
    Vector2 dir;

    private void Awake()
        {
        inputs   = GetComponent<PlayerInputs>();
        animator = GetComponent<Animator>();

        Debug.Assert(inputs != null, "PlayerLogic: No PlayerInputs found on this object!");
        Debug.Assert(animator != null, "PlayerLogic: No Animator found on this object!");
        }

    void OnPunch()
        {
        // a little hacky, but only accept inputs during idle | could queue inputs
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Base Movement"))
            return;

        if (isHeavyPunchReady)
            {
            HeavyPunch();
            return;
            }

        numPresses++;

        LightPunch();
        }

    public void OnPunchCollided()
        {
        if (isPunching)
            {
            Debug.Log("Landed Punch " + landedPunches);
            landedPunches++;

            if (isHeavyPunching)
                onDoDamage?.Invoke(100);
            else
                onDoDamage?.Invoke(10);

            if (landedPunches > 2)
                isHeavyPunchReady = true;
            }
        }

    void LightPunch()
        {
        Debug.Log("LANDED: " + landedPunches);

        // trigger punch
        if (landedPunches > 1)
            {
            animator.SetTrigger("HeavyPunch");
            ClearCombo();
            }
        else if (landedPunches == 1)
            animator.SetTrigger("Punch2");
        else
            animator.SetTrigger("Punch1");

        timer = COMBO_TIMER;
        }

    void HeavyPunch()
        {
        isHeavyPunchReady = false;

        animator.SetTrigger("HeavyPunch");
        
        ClearCombo();
        }

    void Update()
        {
        HandleInputs();

        UpdateTimers();

        CheckOppDistance();
        }

    private void CheckOppDistance()
        {
        // terrible solution, but just keep them from getting too close due to root animation moving them
        Vector3 dir = transform.position - opponent.position;
        if (dir.sqrMagnitude < (closeDistance * closeDistance))
            {
            // force us at a certain distance
            transform.position = opponent.position + dir.normalized * closeDistance;
            }
            
        }

    private void HandleInputs()
        {
        animator.SetFloat("StrafeX", inputs.localMovement.x);
        animator.SetFloat("StrafeZ", inputs.localMovement.z);

        if (inputs.punch)
            OnPunch();
        }

    private void UpdateTimers()
        {
        if (timer <= 0)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0)
            {
            timer = 0;
            ClearCombo();
            }
        }

    void ClearCombo()
        {
        // clear our counter
        numPresses = 0;
        landedPunches = 0;

        Debug.Log("Clearing Combo");
        }

    public void PunchBegin()
        {
        //Debug.Log("Punch Begin");
        isPunching = true;
        }

    public void PunchEnd()
        {
        //Debug.Log("Punch End");
        isPunching = false;
        }

    public void HeavyPunchBegin()
        {
        //Debug.Log("Punch Begin");
        isPunching = true;
        isHeavyPunching = true;
        }

    public void HeavyPunchEnd()
        {
        //Debug.Log("Punch End");
        isPunching = false;
        isHeavyPunching = false;
        }
    }
