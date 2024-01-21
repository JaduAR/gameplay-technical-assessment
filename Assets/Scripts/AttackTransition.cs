using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Attacks/Melee")]
[Serializable]
public class AttackTransition : ScriptableObject
{
    [Tooltip("Animation State To Play")]
    public string AnimationStateName = "";
    [Tooltip("Idle Animation State To Play")]
    public string IdleAnimationStateName = "";
    [Tooltip("Has Another Transition After This Current One")]
    public AttackTransition NextAttackTransition = null;
    [Tooltip("Which Hand Will Be Used For The Attack")]
    public EHand HandIndex = EHand.LEFT;
    [Tooltip("Can This Transition Do Damage")]
    public bool CanDoDamage = false;
    [Tooltip("Amount of Damage")]
    public float Damage = 0.0f;
    [Tooltip("Time In The Animation Normalize Time Can The Collision Be Enabled")]
    public float EnableCollisionAtTime = -1.0f;
}