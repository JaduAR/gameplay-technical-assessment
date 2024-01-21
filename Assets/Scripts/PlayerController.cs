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
        ChargeAttack();
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
        if (attackTimer <= 0) //This is just for preventing input spam
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

                    if (consecutiveHit == maxCombo)
                    {
                        startChargeTimer = true;
                    }
                    hitOnce = true;
                }
            }
        }
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

    private void WaitForCharge()
    {
        if (startChargeTimer)
        {

            chargeWaitTime += Time.deltaTime;

            if (chargeWaitTime >= chargeWaitMaxTime)
            {
                EndWait();
            }
        }
    }

    private void EndWait()
    {
        startChargeTimer = false;
        chargeWaitTime = 0;
    }

    private void ChargeAttack() //Strike, ChargingHS
    {
        WaitForCharge();

        if (attackButtonDown && (startChargeTimer || isCharging))
        {
            attackTimeHeld += Time.deltaTime;

            if (attackTimeHeld >= 0.5f)
            {
                isCharging = true;
                animator.SetBool("IsCharging", true);


            }
        }
    }

    private void HeavyAttack()
    {
        //animator.SetBool("IsCharging", false);
        animator.SetBool("HeavyCharged", true);
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
            HeavyAttack(); //works! Continue here!
        }
        attackButtonDown = false;
        attackTimeHeld = 0;

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