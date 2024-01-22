using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer.FSM;
using Animancer.Units;
using static Animancer.Validate;
using UnityEngine.InputSystem;

public sealed class Controller_StateBases : MonoBehaviour
    {
        [SerializeField] private Character _Character;
        [SerializeField] private StateBase _Attack;
        [SerializeField] private StateHitReact _hitReact;
        [SerializeField] private StateHeavyAttack attackHeavy;
    
        [SerializeField]
        [Seconds(Rule = Value.IsNotNegative)]
        private float _AttackInputTimeOut = 0.5f;
        [Space(20)]
        [SerializeField] SO_Logging logger;
        [SerializeField] public Collider leftCollider;
        [SerializeField] public Collider rightCollider;
    

        bool attack = false;
        public void AttackAction() => attack = true;
        private bool leftHit = false;
        private bool rightHit = false;
        private Vector2 moveVect;
        private StateMachine<StateBase>.InputBuffer _InputBuffer;

    private void Awake()
        {
            ClearHits();
            _InputBuffer = new StateMachine<StateBase>.InputBuffer(_Character.StateMachine);
        }
     private void Update()
        {
            UpdateMovement();
            UpdateCombo();
            UpdateActions();
        }

    private void UpdateMovement()
        {
            Vector3 input = new Vector3(moveVect.x, 0, moveVect.y);
            if (input == default)
            {
                _Character.Parameters.MovementDirection = default;
                return;
            }
            _Character.Parameters.MovementDirection = input;
        }

        public void UpdateMovement_Auto(Vector2 _val)
            {
                moveVect = _val; ;
            }
        public void UpdateMovement_InputAction(InputAction.CallbackContext _context)
            {
                if (_context.performed)
                    {
                        moveVect = _context.ReadValue<Vector2>();
                    }
                else if (_context.canceled)
                    {
                        moveVect = default;
                    }
            }
        public void ClearHits()
            {
                if (!leftCollider || !rightCollider) return;
                leftHit = false;
                rightHit = false;
                leftCollider.gameObject.SetActive(false);
                rightCollider.gameObject.SetActive(false);
            }
        public void LeftHit(Collider col)
            {
                Controller_StateBases sSB = col.transform.parent.GetComponent<Controller_StateBases>();

                if (sSB != null) { sSB.HitReact(0); }
                    leftHit = true;
            }
    
        public void RightHit(Collider col)
        {
            Controller_StateBases sSB = col.transform.parent.GetComponent<Controller_StateBases>();

            if (sSB != null) { sSB.HitReact(0); }
            rightHit = true;
            
        }
        public void ChargedRight(Collider col)
            {
                Controller_StateBases sSB = col.transform.parent.GetComponent<Controller_StateBases>();
                if (sSB != null) { sSB.RemoveAllHP(); sSB.HitReact(1); }
            
            }
        public void HitReact(int _type)
            {
                _hitReact.OnDamageReceived(_type);
            }
        public void RemoveAllHP()
            {
                Debug.Log($"Remove all HP from :{transform.name}");
            }
        public void LeftActive()
            {
                leftCollider.gameObject.SetActive(true);
            }
        public void LeftInactive()
            {
                leftCollider.gameObject.SetActive(false);
            }
        public void RightActive()
            {
                rightCollider.gameObject.SetActive(true);
            }
        public void RightInactive()
            {
                rightCollider.gameObject.SetActive(false);
            }

        void UpdateCombo()
            {
                if(leftHit && rightHit)
                    {
                        ClearHits();
                        attackHeavy.combo = true;
                    }
            }
        private void UpdateActions()
            {
                if (!attackHeavy) return;
                    if(attackHeavy.combo)
                        {
                            _InputBuffer.Buffer(attackHeavy, 0);
                        }

                    if (attack)
                        {
                            _InputBuffer.Buffer(_Attack, _AttackInputTimeOut);
                        }

                _InputBuffer.Update();
                    attack = false;
            }
    }