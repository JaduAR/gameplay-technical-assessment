using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
    {
    public Transform target;
    public float speed;

    // Update is called once per frame
    void Update()
        {
        if (target == null)
            return;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position-transform.position), speed);
        }
    }
