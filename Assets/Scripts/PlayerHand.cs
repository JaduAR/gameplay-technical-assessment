/**********************************************************************************************//**
 * @file    gameplay-technical-assessment\Assets\Scripts\PlayerHand.cs.
 * @brief   Implements the player hand class
 **************************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**********************************************************************************************//**
 * @class   PlayerHand
 * @brief   A player hand.
 * @author  Blake
 * @date    1/21/2024
 **************************************************************************************************/
public class PlayerHand : MonoBehaviour
{
    /** @brief   the player */
    public Player thePlayer = null;
    /**********************************************************************************************//**
     * @enum    HandEnum
     * @brief   Values that represent hand enums
     **************************************************************************************************/
    public enum HandEnum
    {
        ///< An enum constant representing the left hand option
        eLeftHand,
        ///< An enum constant representing the right hand option
        eRightHand
    }

    /** @brief   Type of the hand */
    public HandEnum HandType  = HandEnum.eLeftHand;
    /**********************************************************************************************//**
     * @fn  private void OnTriggerEnter(Collider other)
     * @brief   Executes the trigger enter action
     * @author  Blake
     * @date    1/21/2024
     * @param   other   The other.
     **************************************************************************************************/
    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogFormat("punch landed, hit collider {0}", other.name);

        if (thePlayer != null)
            thePlayer.onHandCollide(HandType, other);
    }

}
