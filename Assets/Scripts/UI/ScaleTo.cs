using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTo : MonoBehaviour
{
    [SerializeField]
    private Vector3 _scaleFrom = Vector3.zero;
    [SerializeField]
    private Vector3 _scaleTo = Vector3.one;
    [SerializeField]
    private float _time = 1.0f;

    private float _currentTime = 0.0f;

    public void Begin()
    {
        _currentTime = 0.0f;
        transform.localScale = _scaleFrom;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentTime < _time)
        {
            _currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, _scaleTo, _currentTime / _time);
        }
    }
}
