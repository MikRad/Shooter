using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D)), RequireComponent(typeof(UnitFxHolder))]
public abstract class BaseUnit : MonoBehaviour, IDamageable
{
    [Header("Attack")]
    [SerializeField] protected float _attackDelay = 1.0f;
    
    [Header("Health")]
    [SerializeField] private int _healthMax;
    
    [Header("Movement")]
    [SerializeField] protected Transform _bodyTransform;

    private int _currentHealth;
    
    private UIProgressBar _healthBar;
    private SpriteRenderer _spriteRenderer;
    protected Collider2D _bodyCollider;
    protected Transform _cachedTransform;
    protected Animator _animator;
    
    protected UnitFxHolder _fxHolder;
    
    private bool HasHealth => _currentHealth > 0;
    protected bool HasMaxHealth => _currentHealth == _healthMax;
    protected float HealthFullness => (float)_currentHealth / _healthMax;
    public bool IsDead => !HasHealth;
    
    protected virtual void Awake()
    {
        _bodyCollider = GetComponent<Collider2D>();
        _healthBar = GetComponentInChildren<UIProgressBar>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _fxHolder = GetComponent<UnitFxHolder>();
        
        _cachedTransform = transform;
    }

    protected virtual void Start()
    {
        FillMaxHealth();
    }

    public virtual void Init(DIContainer diContainer)
    {
        _fxHolder.Init(diContainer.Resolve<AudioController>(), diContainer.Resolve<VfxSpawner>());
    }
    
    public virtual void HandleDamage(int damageAmount)
    {
        RemoveHealth(damageAmount);

        if (!HasHealth)
        {
            Die();
            return;
        }

        _fxHolder.AddDamagedVfx(_cachedTransform.position, _cachedTransform.rotation);
        _fxHolder.PlayDamagedSfx();
    }

    public virtual void Deactivate()
    {
        enabled = false;
    }
    
    protected virtual void Die()
    {
        PlayDeathAnimation();
        _fxHolder.PlayDeathSfx();

        _bodyCollider.enabled = false;
        _spriteRenderer.sortingOrder = 0;
        if(_healthBar != null)
        {
            _healthBar.gameObject.SetActive(false);
        }
    }
    
    private void PlayDeathAnimation()
    {
        _animator.SetTrigger(UnitAnimationIdHelper.GetId(UnitAnimationState.Death));
    }

    private void UpdateHealthBar()
    {
        _healthBar?.SetValue(HealthFullness);
    }
    
    protected void AddHealth(int healthAmount)
    {
        _currentHealth += healthAmount;
        ClampHealthValue();
        UpdateHealthBar();
    }

    protected void FillMaxHealth()
    {
        _currentHealth = _healthMax;
        ClampHealthValue();
        UpdateHealthBar();
    }

    private void RemoveHealth(int healthAmount)
    {
        _currentHealth -= healthAmount;
        ClampHealthValue();
        UpdateHealthBar();
    }
    
    private void ClampHealthValue()
    {
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _healthMax);
    }
}
