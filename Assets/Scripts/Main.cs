/**********************************************************************************************//**
 * @file    gameplay-technical-assessment\Assets\main.cs.
 * @brief   Implements the main class
 **************************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/**********************************************************************************************//**
 * @class   Main
 * @brief   A main.
 * @author  Blake
 * @date    1/18/2024
 **************************************************************************************************/
public class Main : MonoBehaviour
{
   
    /** @brief   /** @brief   the player */
    public Player ThePlayer = null;

    /** @brief   the opponent */
    public Opponent TheOpponent = null;

    public GameObject GameOverScreen = null;

    /** @brief   True to game over */
    bool mGameOver = false;

    /**********************************************************************************************//**
     * @enum    GameEndType
     * @brief   Values that represent game end types
     **************************************************************************************************/
    public enum GameEndType
    {
        ///< An enum constant representing the game beat down option
        eGameBeatDown = 0,
        ///< An enum constant representing the game knockout option
        eGameKnockout = 1,
    }

    /**********************************************************************************************//**
     * @fn  void Start()
     * @brief   Start is called before the first frame update
     * @author  Blake
     * @date    1/18/2024
     **************************************************************************************************/
    void Start()
    {
       
    }
    /**********************************************************************************************//**
     * @fn  void Update()
     * @brief   Update is called once per frame
     * @author  Blake
     * @date    1/18/2024
     **************************************************************************************************/
    void Update()
    {
        if (mGameOver == false)
        {
            //Update player movement
            ThePlayer.updatePlayerMove();

            // update opponent movement
            TheOpponent.updateOpponentMove();

            // check for punch to idle
            ThePlayer.updateAnimState();
        }
        else
        {
            ThePlayer.goStill();

            TheOpponent.goStill();
        }
    }

    /**********************************************************************************************//**
     * @fn  public void restartGame()
     * @brief   Restart game
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    public void restartGame()
    {
        Debug.LogFormat("restartGame !!");
        GameOverScreen.SetActive(false);

        // set the player and opponent back to orig positions
        ThePlayer.resetPlayer();
        TheOpponent.resetOpponent();

        // ok game back on !
        mGameOver = false;
    }

     /**********************************************************************************************//**
      * @fn public void onAttackButtonClicked()
      * @brief  Executes the attack button clicked action
      * @author Blake
      * @date   1/19/2024
      **************************************************************************************************/
     public void onAttackButtonClicked()
    {
        //Debug.LogFormat("onAttackButton Clicked");
        if (mGameOver == false)
        {
            ThePlayer.HandleNewAttack();
        }
    }
    /**********************************************************************************************//**
     * @fn  public void onAttackButtonReleased()
     * @brief   Executes the attack button released action
     * @author  Blake
     * @date    1/21/2024
     **************************************************************************************************/
    public void onAttackButtonReleased()
    {
        //Debug.LogFormat("onAttackButton Released");
        if (mGameOver == false)
        {
            ThePlayer.HandleAttackRelease();
        }
    }
    
    /**********************************************************************************************//**
     * @fn  public void onGameOver(GameEndType endType)
     * @brief   Executes the game over action
     * @author  Blake
     * @date    1/21/2024
     * @param   endType Type of the end.
     **************************************************************************************************/
    public void onGameOver(GameEndType endType)
    {
        Debug.LogFormat("onGameOver END TYPE {0}", endType);

        mGameOver = true;

        Text gameOverText = GameOverScreen.GetComponentsInChildren<Text>()[0];
        if (gameOverText != null)
        {
            if (endType == GameEndType.eGameKnockout)
                gameOverText.text = "Knockout !!!";
            else
                gameOverText.text = "BeatDown...";
        }

        GameOverScreen.SetActive(true);
    }



}
