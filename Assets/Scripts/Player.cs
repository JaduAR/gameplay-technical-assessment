/**********************************************************************************************//**
 * @file    gameplay-technical-assessment\Assets\Scripts\Player.cs.
 * @brief   Implements the player class
 **************************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerHand;
/**********************************************************************************************//**
 * @class   Player
 * @brief   A player.
 * @author  Blake
 * @date    1/21/2024
 **************************************************************************************************/
public class Player : MonoBehaviour
{
    /** @brief   /** @brief   The player */
    public GameObject PlayerObject;
    /** @brief   /** @brief   The player animator */
    Animator mPlayerAnimator;

    /** @brief   /** @brief    the move speed */
    public float MoveSpeed = 0.3f;
    /** @brief   The blend move rate */
    public float mBlendMoveRate = 0.13f;

    /** @brief   The blend strafe x coordinate */
    float mBlendStrafeX = 0;
    /** @brief   The blend strafe z coordinate */
    float mBlendStrafeZ = 0;
    /**********************************************************************************************//**
     * @enum    Player_State
     * @brief   Values that represent player states
     **************************************************************************************************/
    enum Player_State
    {
        ///< An enum constant representing the player idle option
        ePlayerIdle = 0,
        ///< An enum constant representing the player punch 1 option
        ePlayerPunch1,
        ///< An enum constant representing the player punch 2 option
        ePlayerPunch2,
        ///< An enum constant representing the player punch to idle option
        ePlayerPunchToIdle,
        ///< An enum constant representing the player charge option
        ePlayerCharge,
        ///< An enum constant representing the player heavy punch option
        ePlayerHeavyPunch,
    }

    /** @brief   State of the player */
    Player_State mPlayerState = Player_State.ePlayerIdle;
    /** @brief   True to next attack p 1 */
    bool mNextAttackP1 = true;
    /** @brief   The animation start time */
    float mAnimStartTime;

    /** @brief   True to punch 1 hit */
    bool mPunch1Hit = false;
    /** @brief   True to punch 2 hit */
    bool mPunch2Hit = false;

    /** @brief   True to charging heavy punch */
    bool mChargingHeavyPunch;

    /** @brief   The audio source */
    AudioSource mAudioSource;
    /**********************************************************************************************//**
     * @fn  void Start()
     * @brief   Start is called before the first frame update
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    void Start()
    {
        if (PlayerObject != null)
        {
            // grab the player and opponent animators from their game objects
            mPlayerAnimator = PlayerObject.GetComponent<Animator>();
            mAudioSource = PlayerObject.GetComponent<AudioSource>();

            //mPlayerAnimator.applyRootMotion = false;
        }

    }
    /**********************************************************************************************//**
     * @fn  public void reset()
     * @brief   Resets this object
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    public void resetPlayer()
    {
        PlayerObject.transform.position = new Vector3(-0.308f, 0, 1.5f);
        mPunch1Hit = false; 
        mPunch2Hit = false;
        mChargingHeavyPunch = false;
        goStill();
    }

    /**********************************************************************************************//**
     * @fn  public void onHandCollide(HandEnum theHandType, Collider other)
     * @brief   Executes the hand collide action
     * @author  Blake
     * @date    1/21/2024
     * @param   theHandType Type of the hand.
     * @param   other       The other.
     **************************************************************************************************/
    public void onHandCollide(HandEnum theHandType, Collider other)
    {
        //Debug.LogFormat("onHandCollide, hand {0}, hit collider {1}", theHandType, other.name);

        switch (theHandType)
        {
            case HandEnum.eLeftHand:
                if (mPunch1Hit == false && mPlayerState == Player_State.ePlayerPunch1)
                {
                    // new hit from left hand in punch 1
                    mPunch1Hit = true;
                    Debug.LogFormat("onHandCollide NEW LEFT HIT, hand {0}, hit collider {1}", theHandType, other.name);

                    doDamage(other);
                }
                break;
            case HandEnum.eRightHand:
                if (mPlayerState == Player_State.ePlayerHeavyPunch)
                {
                    Debug.LogFormat("onHandCollide KNOCKOUT, hand {0}, hit collider {1}", theHandType, other.name);
                    // we were doing heavy punch so just knock out opponent
                    doKnockout(other);
                }
                else if (mPunch2Hit == false && mPlayerState == Player_State.ePlayerPunch2)
                {
                    // new hit from right hand in punch 2
                    mPunch2Hit = true;
                    Debug.LogFormat("onHandCollide NEW RIGHT HIT, hand {0}, hit collider {1}", theHandType, other.name);

                    doDamage(other);
                }
                break;
        }

    }    
    /**********************************************************************************************//**
     * @fn  void doDamage(Collider other)
     * @brief   Executes the damage operation
     * @author  Blake
     * @date    1/21/2024
     * @param   other   The other.
     **************************************************************************************************/
    void doDamage(Collider other)
    {
        Opponent opp = other.GetComponent<Opponent>();
        if ( (opp != null))
        {
            opp.takePunchDamage();
        }
    }
    /**********************************************************************************************//**
     * @fn  void doKnockout(Collider other)
     * @brief   Executes the knockout operation
     * @author  Blake
     * @date    1/21/2024
     * @param   other   The other.
     **************************************************************************************************/
    void doKnockout(Collider other)
    {
        Opponent opp = other.GetComponent<Opponent>();
        if ((opp != null))
        {
            opp.getKnockedOut();
        }
    }
    /**********************************************************************************************//**
     * @fn  public void updatePlayerMove()
     * @brief   Updates the player move
     * @author  Blake
     * @date    1/18/2024
     **************************************************************************************************/
    public void updatePlayerMove()
    {
        int leftRight = 0;
        int forwardBack = 0;
        
        // check left right keys
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            // left pressed, check right to cancel
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                // they cancel out left right
            }
            else
            {
                // move player to their left
                leftRight = -1;
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            // move player to their right
            leftRight = 1;
        }

        // check forward back keys
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // left pressed, check right to cancel
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                // they cancel out up down
            }
            else
            {
                // move player forward
                forwardBack = -1;
            }
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // move player back
            forwardBack = 1;
        }

        float newStrafeX = MoveSpeed * forwardBack;
        float newStrafeZ = MoveSpeed * leftRight;

        if (Mathf.Sign(newStrafeX) == Mathf.Sign(mBlendStrafeX))
        {
            // normal blend if the track strafeX and the immediate are same sign
            mBlendStrafeX = (newStrafeX * mBlendMoveRate) + (1f - mBlendMoveRate) * mBlendStrafeX;
        }
        else
        {
            // if the tracking strafeX and immediate strafeX are opposite signs, we had change of direction
            mBlendStrafeX = newStrafeX * mBlendMoveRate;
        }

        if (Mathf.Sign(newStrafeZ) == Mathf.Sign(mBlendStrafeZ))
        {
            // normal blend if the track strafeZ and the immediate are same sign
            mBlendStrafeZ = (newStrafeZ * mBlendMoveRate) + (1f - mBlendMoveRate) * mBlendStrafeZ;
        }
        else
        {
            // if the tracking strafeZ and immediate strafeZ are opposite signs, we had change of direction
            mBlendStrafeZ = newStrafeZ * mBlendMoveRate;
        }

        mPlayerAnimator.SetFloat("StrafeX", mBlendStrafeX);
        mPlayerAnimator.SetFloat("StrafeZ", mBlendStrafeZ);

        //Debug.LogFormat("UpdatePlayerMove: mBlendStrafeX : {0}, newStrafeX {1}, forwardBack: {2}", mBlendStrafeX, newStrafeX, forwardBack);
        //Debug.LogFormat("UpdatePlayerMove: mBlendStrafeZ : {0}, newStrafeZ {1}, leftRight: {2}", mBlendStrafeZ, newStrafeZ, leftRight);
    }

    /**********************************************************************************************//**
     * @fn  public void goStill()
     * @brief   Go still
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    public void goStill()
    {
        mPlayerAnimator.SetFloat("StrafeX", 0);
        mPlayerAnimator.SetFloat("StrafeZ", 0);

        mPlayerAnimator.Play("Base Layer.Base Movement");
        mPlayerState = Player_State.ePlayerIdle;
    }

    /**********************************************************************************************//**
     * @fn  public void HandleNewAttack()
     * @brief   Handles the new attack
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    public void HandleNewAttack()
    {
        switch (mPlayerState)
        {
            case Player_State.ePlayerIdle:
                if (CanDoHeavyPunch())
                {
                    // since we in idle , play the charge up directly
                    mPlayerAnimator.Play("Attack.Idle to Charge");
                    mPlayerState = Player_State.ePlayerCharge;
                    mChargingHeavyPunch = true;
                    mAnimStartTime = Time.time;
                }
                else // normal punching
                {
                    if (mNextAttackP1)
                    {
                        mPlayerAnimator.Play("Attack.Idle to P1");
                        mPlayerState = Player_State.ePlayerPunch1;
                        mPunch1Hit = false;
                        mNextAttackP1 = false;
                        mAnimStartTime = Time.time;
                    }
                    else // punch with p2
                    {
                        mPlayerAnimator.Play("Attack.Idle to P2");
                        mPlayerState = Player_State.ePlayerPunch2;
                        mPunch2Hit = false;
                        mNextAttackP1 = true;
                        mAnimStartTime = Time.time;
                    }
                }
                break;
            case Player_State.ePlayerPunch1:
                if (CanDoHeavyPunch())
                {
                    // since we in punch we got to go to idle first before charge up
                    mPlayerAnimator.Play("Attack.P1 to Idle");
                    mPlayerState = Player_State.ePlayerPunchToIdle;
                    mChargingHeavyPunch = true;
                    mAnimStartTime = Time.time;
                }
                else
                {
                    mPlayerAnimator.Play("Attack.P1 to P2");
                    mPlayerState = Player_State.ePlayerPunch2;
                    mPunch2Hit = false;
                    mNextAttackP1 = true;
                    mAnimStartTime = Time.time;
                }
                break;
            case Player_State.ePlayerPunch2:
                if (CanDoHeavyPunch())
                {
                    // since we in punch we got to go to idle first before charge up
                    mPlayerAnimator.Play("Attack.P2 to Idle");
                    mPlayerState = Player_State.ePlayerPunchToIdle;
                    mChargingHeavyPunch = true;
                    mAnimStartTime = Time.time;
                }
                else
                {
                    mPlayerAnimator.Play("Attack.P2 to P1");
                    mPlayerState = Player_State.ePlayerPunch1;
                    mPunch1Hit = false;
                    mNextAttackP1 = false;
                    mAnimStartTime = Time.time;
                }
                break;
        }

        AudioClip clip = (AudioClip)Resources.Load("air-in-a-hit");
        mAudioSource.PlayOneShot(clip);
    }
    /**********************************************************************************************//**
     * @fn  public void HandleAttackRelease()
     * @brief   Handles the attack release
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    public void HandleAttackRelease()
    {
        // check if they got to the charging anim
        if (mPlayerState == Player_State.ePlayerCharge)
        {
            // attack button released and we are charging so play the big punch :)
            mPlayerAnimator.Play("Attack.Charge to Heavy Punch");
            mPlayerState = Player_State.ePlayerHeavyPunch;
            mAnimStartTime = Time.time;

            // clear flags in case we need more punches
            mPunch1Hit = false;
            mNextAttackP1 = true;
        }
        else if (mPlayerState == Player_State.ePlayerPunchToIdle && mChargingHeavyPunch == true)
        {
            // did not finish getting to idle yet before attack button released
            // not sure the right answer but going to make them click and release again  , 
            // could also choose to play the heavy punch anyway??
         
        }
    }
    /**********************************************************************************************//**
     * @fn  bool CanDoHeavyPunch()
     * @brief   Determine if we can do heavy punch
     * @author  Blake
     * @date    1/21/2024
     * @returns True if we can do heavy punch, false if not.
     **************************************************************************************************/
    bool CanDoHeavyPunch()
    {
        if (mPunch1Hit && mPunch2Hit)
            return true;
        else
            return false;
    }
    /**********************************************************************************************//**
     * @fn  public void updateAnimState()
     * @brief   Updates the animation state
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    public void updateAnimState()
    {
        float curTime = Time.time;
        float animTime = curTime - mAnimStartTime;
        if (animTime > 0.5f)
        {
            switch (mPlayerState)
            {
                case Player_State.ePlayerPunch1:
                    //Debug.LogFormat("timeout on ePlayerPunch1");
                    mPlayerAnimator.Play("Attack.P1 to Idle");
                    mPlayerState = Player_State.ePlayerPunchToIdle;
                    mAnimStartTime = Time.time;
                    break;
                case Player_State.ePlayerPunch2:
                    //Debug.LogFormat("timeout on ePlayerPunch2");
                    mPlayerAnimator.Play("Attack.P2 to Idle");
                    mPlayerState = Player_State.ePlayerPunchToIdle;
                    mAnimStartTime = Time.time;
                    break;
                case Player_State.ePlayerHeavyPunch:
                    //Debug.LogFormat("timeout on ePlayerPunch2");
                    mPlayerAnimator.Play("Attack.P2 to Idle");
                    mPlayerState = Player_State.ePlayerPunchToIdle;
                    mAnimStartTime = Time.time;
                    mPunch1Hit = false;
                    mPunch2Hit = false;
                    mChargingHeavyPunch = false;
                    break;
                case Player_State.ePlayerPunchToIdle:
                    if (mChargingHeavyPunch)
                    {
                        // since we now in idle , play the charge up 
                        mPlayerAnimator.Play("Attack.Idle to Charge");
                        mPlayerState = Player_State.ePlayerCharge;
                        mChargingHeavyPunch = true;
                        mAnimStartTime = Time.time;
                    }
                    else
                    { 
                        //Debug.LogFormat("timeout on ePlayerPunchToIdle");
                        mPlayerAnimator.Play("Base Layer.Base Movement");
                        mPlayerState = Player_State.ePlayerIdle;
                        mAnimStartTime = Time.time;
                    }
                    break;
            }
        }
    }

}
