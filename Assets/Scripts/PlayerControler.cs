using UnityEngine;

public class PlayerControler : MonoBehaviour      //attached to the PlayerAvatar object
{
    [Header("Variables")]
    [Tooltip("Player GameObject")]
    public GameObject PlayerGO;

    [Tooltip("Opponent GameObject")]
    public GameObject OpponentGO;

    [Tooltip("Player Animator")]
    private Animator PlayerAnimator;

    [Tooltip("Game Data ")]
    public GameData GD;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = this.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        FaceOpponent();
        Strafe();
    }

    public void FaceOpponent()
    {
        this.transform.LookAt(new Vector3(OpponentGO.transform.position.x, transform.position.y, OpponentGO.transform.position.z));

        //quick hack to fix stafe bug
        PlayerGO.transform.position = new Vector3(PlayerGO.transform.position.x, 0, PlayerGO.transform.position.z);
    }

    public void Strafe()
    {
        PlayerAnimator.SetFloat("StrafeX", Input.GetAxis("Vertical"));
        PlayerAnimator.SetFloat("StrafeZ", Input.GetAxis("Horizontal"));       
    }   

    public void Attack()
    {
        int rnd;
        rnd = UnityEngine.Random.Range(0, 10);

        if (GD.PlayerCombo)
        {          
            PlayerAnimator.SetTrigger("Charge");
            GD.PlayerCharge = true;
        }
        else if (GD.PlayerPunch2Hit)
        {          
            PlayerAnimator.SetTrigger("P2toP1");
            GD.PlayerCombo = true;

        }
        else if (GD.PlayerPunch1Hit)
        {         
            PlayerAnimator.SetTrigger("P1toP2");
            GD.PlayerCombo = true;
        }
        else
        {
            if (rnd > 5)
            {
                PlayerAnimator.SetTrigger("Punch1");
                GD.PlayerPunch1 = true;
            }
            else
            {
                PlayerAnimator.SetTrigger("Punch2");
                GD.PlayerPunch2 = true;
            }
        }        
    }
}

