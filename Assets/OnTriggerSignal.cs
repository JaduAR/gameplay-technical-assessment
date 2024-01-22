using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerSignal : MonoBehaviour
    {
    public UnityEvent<Collider> triggerEntered;
    
    private void OnTriggerEnter(Collider other)
        {
        triggerEntered?.Invoke(other);
        }
    }
