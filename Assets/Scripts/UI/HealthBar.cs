using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image _barInnerImage = null;
    [SerializeField]
    private float _time = 1.0f;

    private float _currentTime = 0.0f;
    private float _currentValue = 0.0f;
    private float _finalValue = 0.0f;

    private void Update()
    {
        if (_currentTime < _time)
        {
            _currentTime += Time.deltaTime;
            _barInnerImage.fillAmount = Mathf.Lerp(_barInnerImage.fillAmount, _finalValue, _currentTime / _time);
        }
    }

    public void SetValue(float value)
    {
        _currentValue = value;
        _finalValue = value;
        _currentTime = _time;
        _barInnerImage.fillAmount = _currentValue;
    }

    public void UpdateLerpValueTo(float value)
    {
        _currentTime = 0.0f;
        _finalValue = value;
    }
}
