using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int _healthMax;

    private int _currentHealth;
    private UIProgressBar _healthBar;

    public bool HasHealth => _currentHealth > 0;
    public bool HasMaxHealth => _currentHealth == _healthMax;

    public float HealthFullness => (float)_currentHealth / _healthMax;

    private void Awake()
    {
        _healthBar = GetComponentInChildren<UIProgressBar>();
    }

    public void HideHealthBar()
    {
        if(_healthBar != null)
        {
            _healthBar.gameObject.SetActive(false);
        }
    }

    public void ChangeHealth(int healthAmountDelta)
    {
        _currentHealth += healthAmountDelta;
        ClampHealthValue();
        UpdateHealthBar();
    }

    public void FillMaxHealth()
    {
        _currentHealth = _healthMax;
        ClampHealthValue();
        UpdateHealthBar();
    }
    
    private void UpdateHealthBar()
    {
        if (_healthBar != null)
        {
            _healthBar.SetValue(HealthFullness);
        }
    }
    
    private void ClampHealthValue()
    {
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _healthMax);
    }
}
