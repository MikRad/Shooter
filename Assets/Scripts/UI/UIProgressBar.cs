using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _progressImage;
        [SerializeField] private float _valueChangeTime = 1.5f;

        private float _targetValue = 1f;
        private float _currentValueChangeTime;

        private void Update()
        {
            if (_currentValueChangeTime < _valueChangeTime)
            {
                _currentValueChangeTime += Time.deltaTime;
                _progressImage.fillAmount = Mathf.Lerp(_progressImage.fillAmount, _targetValue, _currentValueChangeTime / _valueChangeTime);
            }
        }

        public void SetValue(float value)
        {
            value = Mathf.Clamp(value, 0f, 1f);

            _targetValue = value;
            _currentValueChangeTime = 0f;
        }
    }
}