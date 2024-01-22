using System.Collections;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Collider[] attackColliders;
    int health = 100;
    int damage = 10;

    private bool isAttacking;
    float attackCD = 0f;
    float attackTimer;


    private bool punchPressed;
    private bool leftPunch = true;
    float punchMinHoldTime = 0.5f;

    /// <summary>
    /// Combo Vars
    /// </summary>
    private bool inCombo;
    private int maxCombo = 2;
    private int comboCounter;
    private int consecutiveHit;
    private float comboTimer;
    private float comboMaxTime = 1f;


    /// <summary>
    /// Charge Vars
    /// </summary>
    private bool isCharging;
    private bool waitForCharge;
    float waitForChargeTimer;
    float waitForChargeMax = 1f;
    float chargeHeldTime;
    float uppercutChargeMax = 1.25f;

    private Opponent opponent;


    private void Start()
    {
        opponent = FindObjectOfType<Opponent>();
    }

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
        if (!isAttacking && chargeHeldTime < punchMinHoldTime)
        {
            isAttacking = true;
        }

        if (isCharging)
        {
            isCharging = false;
            animator.SetBool("IsCharging", false);
        }
        if (chargeHeldTime >= uppercutChargeMax)
        {
            animator.Play("Charge to Heavy Punch");
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

        transform.LookAt(opponent.transform);

        rb.velocity = (transform.forward * inputHor + transform.right * -inputVer).normalized;
    }


    private void HandleAttack()
    {
        if (attackTimer <= 0)
        {
            if (isAttacking)
            {
                Punch();
                CheckForHit();
                attackTimer = attackCD;
                isAttacking = false;
                //Punch();
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
        comboCounter++;
        animator.SetBool("InCombo", true);
        leftPunch = !leftPunch;
        animator.SetBool("LeftPunch", leftPunch);
        StartCoroutine(ResetPunchTrigger());

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

    }

    private void CheckForHit()
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
                    consecutiveHit++;

                    if (consecutiveHit >= maxCombo)
                    {
                        waitForCharge = true;
                    }
                    hitOnce = true;
                }
            }
        }
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
        waitForChargeTimer = 0;
    }
}