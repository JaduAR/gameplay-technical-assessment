using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackButtonEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerController _playerController;

    private Animator _attackButtonAnim;
    private bool _startChargeAnimation = false;

    private void Awake()
    {
        _attackButtonAnim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if(_playerController._chargeComplete)
        {
            if (!_startChargeAnimation)
            {
                _attackButtonAnim.SetBool("ChargeButton", true);
                _startChargeAnimation = true;
            }            
        }
        else
        {
            if (_startChargeAnimation)
            {
                _attackButtonAnim.SetBool("ChargeButton", false);
                _startChargeAnimation = false;
            }            
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _playerController.HandleAttack();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _playerController.HandleRelease();
    }
}
