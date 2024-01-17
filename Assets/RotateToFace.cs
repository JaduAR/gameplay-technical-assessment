using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToFace : MonoBehaviour
{

    [SerializeField] private GameObject _enemy;

    private void Update()
    {
        transform.LookAt(new Vector3(_enemy.transform.position.x, transform.position.y, _enemy.transform.position.z));
    }
}
