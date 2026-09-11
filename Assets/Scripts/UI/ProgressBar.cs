using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private TextMeshProUGUI _progressText;

    private float _maxValue;
    private float _currentValue;

    public void InitializeProgressBar(float current, float maxValue)
    {
        _maxValue = maxValue;
        SetValue(current);
    }

    public void SetValue(float current)
    {
        _currentValue = current;
        _fillImage.fillAmount = Mathf.Clamp01(_currentValue / _maxValue);
        if (_progressText != null)
        {
            _progressText.text = $"{_currentValue}     /     {_maxValue}";
        }
    }
}