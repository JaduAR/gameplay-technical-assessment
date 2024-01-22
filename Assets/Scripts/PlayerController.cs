using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    float punchPressedTime;
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
    private bool isCharging;
    private bool waitForCharge;
    float waitForChargeTimer;
    float waitForChargeMax = 1f;
    float uppercutChargeMax = 1.25f;



    private Rigidbody rb;
    private Animator animator;
    private Opponent opponent;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
        if (!isAttacking && punchPressedTime < punchMinHoldTime)
        {
            isAttacking = true;
        }

        if (isCharging)
        {
            isCharging = false;
            animator.SetBool("IsCharging", false);
        }
        if (punchPressedTime >= uppercutChargeMax)
        {
            animator.SetBool("UppercutReady", true);
        }
        else animator.SetBool("UppercutReady", false);

        punchPressed = false;
        punchPressedTime = 0;

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
                attackTimer = attackCD;
                animator.SetTrigger("Punch");

                inCombo = true;
                comboTimer = comboMaxTime;
                comboCounter++;
                animator.SetBool("InCombo", true);
                CheckForHit();
                isAttacking = false;
                leftPunch = !leftPunch;
                animator.SetBool("LeftPunch", leftPunch);

                //Punch();
            }
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
        HandleCharge();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("P1 to P2") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("P2 to P1"))
        {
            animator.ResetTrigger("Punch");
        }
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
            punchPressedTime += Time.deltaTime;

            if (punchPressedTime >= punchMinHoldTime && !isCharging)
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