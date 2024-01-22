using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static void CreatePunchEffect(Transform spawnTransform)
    {
        var attackEffect = Instantiate(GameManager.i.assetHolder.attackEffectPrefab, spawnTransform.position, Quaternion.identity);
    }
}
