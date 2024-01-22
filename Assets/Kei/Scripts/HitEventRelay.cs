using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class CollisonEnterEvent : UnityEvent<Collision> { }
[System.Serializable] public class TriggerEnterEvent : UnityEvent<Collider> { }
[System.Serializable] public class CollisonExitEvent : UnityEvent<Collision> { }
[System.Serializable] public class TriggerExitEvent : UnityEvent<Collider> { }
[System.Serializable] public class CollisonStayEvent : UnityEvent<Collision> { }
[System.Serializable] public class TriggerStayEvent : UnityEvent<Collider> { }

public class HitEventRelay : MonoBehaviour
{
    public CollisonEnterEvent collisonEnterEvent;
    public TriggerEnterEvent triggerEnterEvent;
    public CollisonExitEvent collisonExitEvent;
    public TriggerExitEvent triggerExitEvent;
    public TriggerStayEvent triggerStayEvent;

    public bool _enabled = true;

    [SerializeField] bool doWhileDisabled = false; 

    public bool DoWhileDisable
        {
            get { return doWhileDisabled; }
            set { doWhileDisabled = value; }
        }
    private void OnCollisionEnter(Collision collision)
        {
        if (!_enabled) return;
        if(isActiveAndEnabled || doWhileDisabled)
            collisonEnterEvent.Invoke(collision);
        }
    private void OnTriggerEnter(Collider other)
        {
        if (!_enabled) return;
        if (isActiveAndEnabled || doWhileDisabled)
            triggerEnterEvent.Invoke(other);
        }

    private void OnTriggerStay(Collider other)
        {
        if (!_enabled) return;
        if (isActiveAndEnabled || doWhileDisabled)
            triggerStayEvent.Invoke(other);
        }

    private void OnCollisionExit(Collision collision)
        {
        if (!_enabled) return;
        if (isActiveAndEnabled || doWhileDisabled)
            collisonExitEvent.Invoke(collision);
        }

    private void OnTriggerExit(Collider other)
        {
        if (!_enabled) return;
        if (isActiveAndEnabled || doWhileDisabled)
            triggerExitEvent.Invoke(other);
        }

    }
