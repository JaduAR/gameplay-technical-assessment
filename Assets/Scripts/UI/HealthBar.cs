using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Tag")]
    [SerializeField]
    private string _tag;

    [Header("Image")]
    [SerializeField]
    private Image _lifeBarImage;

    [Header("Min / Max")]
    [SerializeField]
    private int _min = 0;
    [SerializeField]
    private int _max = 100;

    private void OnEnable()
    {
        PlayerHealth.OnHealtheUpdate += UpdateHealthBar;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealtheUpdate -= UpdateHealthBar;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnHealtheUpdate -= UpdateHealthBar;
    }

    private void UpdateHealthBar(string tag, int value)
    {
        if (_lifeBarImage == null ||
            value < _min ||
            tag != _tag)
        {
            return;
        }

        _lifeBarImage.fillAmount = (1f / _max) * value ;
    }
}
