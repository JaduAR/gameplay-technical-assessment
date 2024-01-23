/**********************************************************************************************//**
 * @file    gameplay-technical-assessment\Assets\Scripts\DownClickButton.cs.
 * @brief   Implements the down click button class
 **************************************************************************************************/
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
/**********************************************************************************************//**
 * @class   DownClickButton
 * @brief   A down click button.
 * @author  Blake
 * @date    1/21/2024
 **************************************************************************************************/
public class DownClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /** @brief   The on down click */
    public UnityEvent onDownClick;
    /** @brief   The on up click */
    public UnityEvent onUpClick;
    /**********************************************************************************************//**
     * @fn  public void OnPointerDown(PointerEventData eventData)
     * @brief   Executes the pointer down action
     * @author  Blake
     * @date    1/21/2024
     * @param   eventData   Information describing the event.
     **************************************************************************************************/
    public void OnPointerDown(PointerEventData eventData)
    {
        if (onDownClick != null)
            onDownClick.Invoke();
    }
    /**********************************************************************************************//**
     * @fn  public void OnPointerUp(PointerEventData eventData)
     * @brief   Executes the pointer up action
     * @author  Blake
     * @date    1/21/2024
     * @param   eventData   Information describing the event.
     **************************************************************************************************/
    public void OnPointerUp(PointerEventData eventData)
    {
        if (onUpClick != null)
            onUpClick.Invoke();
    }


}