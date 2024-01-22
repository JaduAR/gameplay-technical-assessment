/**********************************************************************************************//**
 * @file    gameplay-technical-assessment\Assets\Scripts\Opponent.cs.
 * @brief   Implements the opponent class
 **************************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**********************************************************************************************//**
 * @class   Opponent
 * @brief   An opponent.
 * @author  Blake
 * @date    1/21/2024
 **************************************************************************************************/
public class Opponent : MonoBehaviour
{
    /** @brief   the main */
    public Main theMain;

    /** @brief   /** @brief   The opponent */
    public GameObject OpponentObject;

    /** @brief   The player object */
    public GameObject PlayerObject;

    /** @brief   /** @brief   The opponent animator */
    Animator mOpponentAnimator;

    /** @brief   The health */
    int mHealth = 100;

    /** @brief   True to move left of player */
    bool mMoveLeftOfPlayer = false;


    /** @brief   /** @brief    the move speed */
    public float MoveSpeed = 0.3f;
    /** @brief   The blend move rate */
    public float mBlendMoveRate = 0.13f;

    /** @brief   a i minimum move time */
    public float mAIMinMoveTime = 1f;
    /** @brief   a i move time random */
    public float mAIMoveTimeRandom = 4f;


    /** @brief   The blend strafe x coordinate */
    float mBlendStrafeX = 0;
    /** @brief   The blend strafe z coordinate */
    float mBlendStrafeZ = 0;

    /** @brief   a i move type start time */
    float mAIMoveTypeStartTime;
    /** @brief   a i move duration time */
    float mAIMoveDurationTime = 0.5f;

    AudioSource mAudioSource;
    /**********************************************************************************************//**
     * @fn  void Start()
     * @brief   Start is called before the first frame update
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    void Start()
    {
        if (OpponentObject != null)
        {
            mOpponentAnimator = OpponentObject.GetComponent<Animator>();
            mAudioSource = OpponentObject.GetComponent<AudioSource>();

        }

        mMoveLeftOfPlayer = true;
        mAIMoveTypeStartTime = Time.time;
    }
    /**********************************************************************************************//**
     * @fn  public void takePunchDamage()
     * @brief   Take punch damage
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    public void takePunchDamage()
    {
        mHealth -= 10;   

        AudioClip clip = (AudioClip)Resources.Load("boxer-getting-hit");
        mAudioSource.PlayOneShot(clip);

        if (mHealth <= 0)
        {
            theMain.onGameOver(Main.GameEndType.eGameBeatDown);
        }
    }
    /**********************************************************************************************//**
     * @fn  public void getKnockedOut()
     * @brief   Gets knocked out
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    public void getKnockedOut()
    {
        AudioClip clip = (AudioClip)Resources.Load("cartoon-dazzle");
        mAudioSource.PlayOneShot(clip);

        theMain.onGameOver(Main.GameEndType.eGameKnockout);
    }
    /**********************************************************************************************//**
     * @fn  public void updateOpponentMove()
     * @brief   Updates the opponent move
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    public void updateOpponentMove()
    {
        int leftRight = 0;
        int forwardBack = 0;

        // first we check time to switch left / right position from player to add back and forth
        float moveTime = Time.time - mAIMoveTypeStartTime;
        if (moveTime > mAIMoveDurationTime)
        {
            mMoveLeftOfPlayer = !mMoveLeftOfPlayer;
            mAIMoveTypeStartTime = Time.time;

            mAIMoveDurationTime = mAIMinMoveTime + Random.Range(0, mAIMoveTimeRandom);
        }

        // get distance vector between opponent and player
        Vector3 diff = OpponentObject.transform.position - PlayerObject.transform.position;

        // should AI , move left right on screen to get closer or farther in X
        if (diff.x < 0.6)
        {
            // too close in X, move away in X
            leftRight = -1;
        }
        else if (diff.x >0.8)
        {
            // too far away move closer in X
            leftRight = 1;
        }
        else
        {
            // no move in X, X dist is good for Opponent logic

        }

        // should AI , move forward back(into / out) of screen to get closer or farther in Z
        // first check if move to left or right of the player
        if (mMoveLeftOfPlayer == true)
        {
            // try to stay left of the player
            if (diff.z > 0.5)
            {
                // too close in Z, move away in Z
                forwardBack = -1;
            }
            else if (diff.z < 0.3)
            {
                // too far away move closer in Z
                forwardBack = 1;
            }
            else
            {
                // no move in Z, Z dist is good for Opponent logic

            }
        }
        else
        {
            // try to stay right of the player
            if (diff.z < -0.5)
            {
                // too close in Z, move away in Z
                forwardBack = 1;
            }
            else if (diff.z > -0.3)
            {
                // too far away move closer in Z
                forwardBack = -1;
            }
            else
            {
                // no move in Z, Z dist is good for Opponent logic

            }
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

        mOpponentAnimator.SetFloat("StrafeX", mBlendStrafeX);
        mOpponentAnimator.SetFloat("StrafeZ", mBlendStrafeZ);

    }
    /**********************************************************************************************//**
     * @fn  public void goStill()
     * @brief   Go still
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    public void goStill()
    {
        mOpponentAnimator.SetFloat("StrafeX", 0);
        mOpponentAnimator.SetFloat("StrafeZ", 0);
        mOpponentAnimator.Play("Base Layer.Base Movement");
    }

    /**********************************************************************************************//**
     * @fn  public void resetOpponent()
     * @brief   Resets the opponent
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    public void resetOpponent()
    {
        OpponentObject.transform.position = new Vector3(0.355f, 0, 1.5f);
        mMoveLeftOfPlayer = true;
        mAIMoveTypeStartTime = Time.time;
        mHealth = 100;

        goStill();
    }
}
