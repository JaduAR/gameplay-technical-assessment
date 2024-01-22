using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : Character
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Collider[] attackColliders;
    [SerializeField] AttackSO[] attackMoves;
    private AttackSO currentMove;
    private List<AttackSO> movesInCombo = new List<AttackSO>();

    //private bool isAttacking;
    float attackCD = 0f;
    float attackTimer;


    private bool punchPressed;
    private bool leftPunch = true;
    float punchMinHoldTime = 0.5f;

    /// <summary>
    /// Combo Vars
    /// </summary>
    private AttackSO readyCombo;
    private bool inCombo;
    private int maxCombo = 2;
    private int consecutiveHit;
    private float comboTimer;
    private float comboMaxTime = 0.5f;


    /// <summary>
    /// Charge Vars
    /// </summary>
    private bool isCharging;
    private bool waitForCharge;
    float waitForChargeTimer;
    float waitForChargeMax = 1f;
    float chargeHeldTime;
    float uppercutChargeMax = 1.25f;

    private void Update()
    {
        HandleMovement();
        HandleAttack();
        ComboTimer();
    }

    public void OnAttackDown()
    {
        punchPressed = true;
    }

    public void OnAttackUp()
    {
        if (!currentMove && chargeHeldTime < punchMinHoldTime)
        {
            currentMove = attackMoves[0];
        }

        if (isCharging)
        {
            isCharging = false;
            animator.SetBool("IsCharging", false);
        }
        if (chargeHeldTime >= uppercutChargeMax)
        {
            Uppercut();
        }

        punchPressed = false;
        chargeHeldTime = 0;

    }

    private void HandleMovement()
    {
        float inputHor = Input.GetAxis("Horizontal");
        float inputVer = Input.GetAxis("Vertical");

        animator.SetFloat("StrafeX", -inputVer);
        animator.SetFloat("StrafeZ", inputHor);

        transform.LookAt(GameManager.i.opponent.transform);

        rb.velocity = (transform.forward * inputHor + transform.right * -inputVer).normalized;
    }


    private void HandleAttack()
    {
        if (attackTimer <= 0)
        {
            if (currentMove == attackMoves[0])
            {
                Punch();
                CheckForHit(attackMoves[0].damage);
                attackTimer = attackCD;
                currentMove = null;
            }
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
        HandleCharge();
    }

    private IEnumerator ResetPunchTrigger()
    {
        yield return new WaitForSeconds(0.1f);
        animator.ResetTrigger("Punch");
    }

    private void Punch()
    {
        animator.SetTrigger("Punch");

        inCombo = true;
        comboTimer = comboMaxTime;
        animator.SetBool("InCombo", true);
        leftPunch = !leftPunch;
        animator.SetBool("LeftPunch", leftPunch);
        StartCoroutine(ResetPunchTrigger());

    }

    private void Uppercut()
    {
        animator.Play("Charge to Heavy Punch");
        currentMove = attackMoves[1];
        CheckForHit(attackMoves[1].damage);
        currentMove = null;
    }

    private void ComboTimer()
    {
        if (inCombo)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0)
                EndCombo();
        }
    }

    private void EndCombo()
    {
        inCombo = false;
        consecutiveHit = 0;
        animator.SetBool("InCombo", false);
        movesInCombo.Clear();
        readyCombo = null;

    }

    private void CheckForHit(int damage)
    {

        for (int i = 0; i < attackColliders.Length; i++)
        {
            var cols = Physics.OverlapBox(attackColliders[i].bounds.center, attackColliders[i].bounds.extents, attackColliders[i].transform.rotation);
            bool hitOnce = false;
            foreach (Collider hit in cols)
            {
                if (hit.transform.root == transform)
                    continue;
                if (!hitOnce && consecutiveHit < maxCombo)
                {
                    GameManager.i.opponent.TakeDamage(damage, attackColliders[i].transform);
                    movesInCombo.Add(currentMove);
                    consecutiveHit++;
                    if (ComboExists() && readyCombo.isChargeable)
                    {
                        waitForCharge = true;
                        GameManager.i.uiManager.ChargeReadyTextActive(true);
                    }
                    hitOnce = true;
                }
            }
        }
    }

    private bool ComboExists()
    {
        foreach (AttackSO move in attackMoves)
        {
            if (move.RequiredCombo.SequenceEqual(movesInCombo))
            {
                readyCombo = move;
                return true;
            }
        }
        return false;
    }


    private void StartChargeTimer()
    {
        if (waitForCharge)
        {
            waitForChargeTimer += Time.deltaTime;

            if (waitForChargeTimer >= waitForChargeMax)
            {
                EndChargeTimer();
            }
        }
    }

    private void HandleCharge()
    {
        StartChargeTimer();

        if (punchPressed && (waitForCharge || isCharging))
        {
            chargeHeldTime += Time.deltaTime;

            if (chargeHeldTime >= punchMinHoldTime && !isCharging)
            {
                StartCharging();
            }
        }
    }

    private void StartCharging()
    {
        if(!isCharging)
            animator.SetBool("IsCharging", true);

        isCharging = true;
        EndChargeTimer(); // Reset charge-related variables
    }

    private void EndChargeTimer()
    {
        waitForCharge = false;
        GameManager.i.uiManager.ChargeReadyTextActive(false);
        waitForChargeTimer = 0;
    }

    public void ActivateColliders(string whichPart) //Called by Animation with Events
    {
        var collider = attackColliders[0];
        switch (whichPart)
        {
            case "LeftPunch":
                collider = attackColliders[0];
                
            break;

            case "RightPunch":
                collider = attackColliders[1];
               
                break;
        }

        //collider.enabled = true;
        //StartCoroutine(DeactivateColliders(collider));
    }
    //Not working perfectly yet
    /*public IEnumerator DeactivateColliders(Collider collider)
    {
        yield return new WaitForEndOfFrame();

        collider.enabled = false;
    }*/ 
}