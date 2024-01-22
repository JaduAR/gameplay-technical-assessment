using System.Collections;
using UnityEngine;

public enum AttackState
{
    None,
    Punch,
    DoublePunch,
    Charging,
    Uppercut
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Collider[] attackColliders;
    int health = 100;
    int damage = 10;

    private bool isAttacking;
    private bool punchPressed;
    float attackCD = 0f;
    float attackTimer;

    float punchPressedTime;

    private Coroutine attackCoroutine;

    /// <summary>
    /// Combo Vars
    /// </summary>
    private int maxCombo = 2;
    private int consecutiveHit;
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
    }

    public void OnAttackDown()
    {
        punchPressed = true;
    }

    public void OnAttackUp()
    {
        if (!isAttacking && punchPressedTime <= 0.5f)
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
        if (punchPressedTime >= uppercutChargeMax)
        {
            animator.SetBool("UppercutReady", true);
        }
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
                animator.SetBool("Punch", true);
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

            if (punchPressedTime >= 0.5f && !isCharging)
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
        waitForCharge = false;
        waitForChargeTimer = 0;
    }

    IEnumerator ExitAttack(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("Punch", false);
        isAttacking = false;
        attackCoroutine = null;
        consecutiveHit = 0;
    }
}