using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Logger", menuName = "SO/Logging")]
public class SO_Logging : ScriptableObject
{
    [SerializeField] bool enable;

    public void Log(object _string, Object _sender = null)
        {
            if(!enable) return;
                Debug.Log(_string, _sender);
        }
}