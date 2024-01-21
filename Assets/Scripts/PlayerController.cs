using System.Collections;
using UnityEngine;

public enum AttackState
{
    Idle,
    Attack,
    Charging,
    HeavyAttack
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Collider[] attackColliders;
    int health = 100;
    int damage = 10;

    private bool isAttacking;
    private bool attackButtonDown;
    float attackCD = 1f;
    float attackTimer;

    float attackTimeHeld;

    private Coroutine attackCoroutine;

    /// <summary>
    /// Combo Vars
    /// </summary>
    int maxCombo = 2;
    private bool startChargeTimer;
    private bool isCharging;
    private int consecutiveHit;
    float chargeWaitTime;
    float chargeWaitMaxTime = 1f;
    float chargingMaxTime = 1.5f;



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
    }

    public void OnAttackDown()
    {
        attackButtonDown = true;
    }

    public void OnAttackUp()
    {
        if (!isAttacking && attackTimeHeld <= 0.5f)
        {
            isAttacking = true;
        }
        else
        {
            CheckForCombo();
        }

        if (isCharging)
        {
            isCharging = false;
            animator.SetBool("IsCharging", false);
        }
        if (attackTimeHeld >= chargingMaxTime)
        {
            animator.SetBool("HeavyCharged", true);
        }
        attackButtonDown = false;
        attackTimeHeld = 0;

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
                animator.SetBool("Attack", true);
                attackTimer = attackCD;

                CheckForHit();

                if (attackCoroutine == null)
                {
                    attackCoroutine = StartCoroutine(ExitAttack(0.2f));
                }
            }
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
        HandleCharge();
    }

    private void CheckForCombo()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle to P1") && isAttacking)
        {
            if (attackCoroutine != null)
                StopCoroutine(attackCoroutine);
            CheckForHit();
            attackCoroutine = StartCoroutine(ExitAttack(animator.GetCurrentAnimatorStateInfo(0).length));
        }
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
                        startChargeTimer = true;
                    }
                    hitOnce = true;
                }
            }
        }
    }

    private void StartChargeTimer()
    {
        if (startChargeTimer)
        {
            chargeWaitTime += Time.deltaTime;

            if (chargeWaitTime >= chargeWaitMaxTime)
            {
                EndChargeTimer();
            }
        }
    }

    private void HandleCharge()
    {
        StartChargeTimer();

        if (attackButtonDown && (startChargeTimer || isCharging))
        {
            attackTimeHeld += Time.deltaTime;

            if (attackTimeHeld >= 0.5f && !isCharging)
            {
                StartCharging();
            }
        }
    }

    private void StartCharging()
    {
        isCharging = true;
        animator.SetBool("IsCharging", true);
        EndChargeTimer(); // Reset charge-related variables
    }

    private void EndChargeTimer()
    {
        startChargeTimer = false;
        chargeWaitTime = 0;
    }

    IEnumerator ExitAttack(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("Attack", false);
        isAttacking = false;
        attackCoroutine = null;
        consecutiveHit = 0;
    }
}