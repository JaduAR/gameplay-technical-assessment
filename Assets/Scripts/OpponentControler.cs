using System.Collections;
using UnityEngine;

public class OpponentControler : MonoBehaviour      //attached to the OpponentAvatar object
{
    public enum PlayerLoc { None, North, East, South, West }

    [Header("Variables")]
    [Tooltip("Player location ")]
    public PlayerLoc PlayerCurLoc = PlayerLoc.None;

    [Tooltip("Player GameObject")]
    public GameObject PlayerGO;

    [Tooltip("Opponent GameObject")]
    public GameObject OpponentGO; 

    [Tooltip("Game Data ")]
    public GameData GD;

    private Animator OpponentAnimator;

    private bool IsMoving;

    private int rndDir;


    // Start is called before the first frame update
    void Start()
    {
        OpponentAnimator = this.GetComponent<Animator>();    
    }   

    void FixedUpdate()
    {
        FacePlayer();
        Strafe();
    }

    void FacePlayer()
    {
        this.transform.LookAt(new Vector3(PlayerGO.transform.position.x, transform.position.y, PlayerGO.transform.position.z));

        //quick hack to fix stafe bug
        OpponentGO.transform.position = new Vector3(OpponentGO.transform.position.x, 0, OpponentGO.transform.position.z);      
    }

    public void OnTriggerEnter(Collider other)
    {       
        if ((other.name == "PlayerAvatar") && (GD.PlayerPunch1 == true))
        {            
            GD.PlayerPunch1 = false;

            GD.OpponentHealth -= 10;

            GD.PlayerPunch1Hit = true;         
        }

        if ((other.name == "PlayerAvatar") && (GD.PlayerPunch2 == true))
        {
            GD.PlayerPunch2 = false;

            GD.OpponentHealth -= 10;

            GD.PlayerPunch2Hit = true;        
        }

        if ((other.name == "PlayerAvatar") && (GD.PlayerCharge == true))
        {
            GD.PlayerCharge = false;

            GD.OpponentHealth = 0;               
        }
    }

    public void OnTriggerExit(Collider other)
    {       
        if (other.name == "PlayerAvatar")
        {
            OpponentAnimator.SetFloat("StrafeZ", 0.0f);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.name == "PlayerAvatar")
        {           
            OpponentAnimator.SetFloat("StrafeZ", -1.0f);
        }
    }

    public void Strafe()
    {
        if (IsMoving) return;       

        rndDir = Random.Range(0, 4);     

        if (PlayerCurLoc == PlayerLoc.North)
        {
            rndDir = 1;
        }
        else if (PlayerCurLoc == PlayerLoc.East)
        {
            rndDir = 3;
        }
        else if (PlayerCurLoc == PlayerLoc.South)
        {
            rndDir = 0;
        }
        else if (PlayerCurLoc == PlayerLoc.West)
        {
            rndDir = 2;
        }

        if (rndDir == 0)
        {
            IsMoving = true;
            OpponentAnimator.SetFloat("StrafeX", 1);
            StartCoroutine(MovementStop("StrafeX"));
        }
        else if (rndDir == 1)
        {
            IsMoving = true;
            OpponentAnimator.SetFloat("StrafeX", -1);
            StartCoroutine(MovementStop("StrafeX"));
        }
        else if (rndDir == 2)
        {
            IsMoving = true;
            OpponentAnimator.SetFloat("StrafeY", 1);
            StartCoroutine(MovementStop("StrafeY"));
        }
        else if (rndDir == 3)
        {
            IsMoving = true;
            OpponentAnimator.SetFloat("StrafeX", -1);
            StartCoroutine(MovementStop("StrafeX"));
        }
    }

    IEnumerator MovementStop(string whichStrafe)
    {
        float waitDir;

        waitDir = Random.Range(0f, 2.0f);

        yield return new WaitForSeconds(waitDir);
        OpponentAnimator.SetFloat(whichStrafe, 0);
        float waitTime;

        waitTime = Random.Range(0f, 2.0f); 

        yield return new WaitForSeconds(waitTime);

        IsMoving = false;
    }
}

