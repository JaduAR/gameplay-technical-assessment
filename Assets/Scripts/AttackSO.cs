using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attacks", menuName = "ScriptableObjects/AttackMove", order = 1)]
public class AttackSO : ScriptableObject
{
    public int damage;
    public List<AttackSO> RequiredCombo = new List<AttackSO>();
    public bool isChargeable;
}
