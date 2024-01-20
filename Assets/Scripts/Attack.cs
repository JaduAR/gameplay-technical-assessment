using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Attacks/Melee")]
[Serializable]
public class Attack : ScriptableObject
{
    public string AnimationStateName = "";
    public string IdleAnimationStateName = "";
    public Attack NextAttackTransition = null;
    public EHand HandIndex = EHand.LEFT;
    public bool CanDoDamage = false;
    public float Damage = 0.0f;
    public float EnableCollisionAtTime = -1.0f;
}