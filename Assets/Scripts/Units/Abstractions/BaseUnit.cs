using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D)), 
 RequireComponent(typeof(UnitHealth)), RequireComponent(typeof(UnitFxHolder))]
public abstract class BaseUnit : MonoBehaviour, IDamageable
{
    [Header("Attack")]
    [SerializeField] protected float _attackDelay = 1.0f;
    
    [Header("Movement")]
    [SerializeField] protected Transform _bodyTransform;

    protected UnitHealth _health;
    private SpriteRenderer _spriteRenderer;
    protected Collider2D _bodyCollider;
    protected Transform _cachedTransform;
    protected Animator _animator;
    
    protected UnitFxHolder _fxHolder;
    
    public bool IsDead => !_health.HasHealth;
    
    protected virtual void Awake()
    {
        _bodyCollider = GetComponent<Collider2D>();
        _health = GetComponent<UnitHealth>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _fxHolder = GetComponent<UnitFxHolder>();
        
        _cachedTransform = transform;
    }

    protected virtual void Start()
    {
        _health.FillMaxHealth();
    }

    public virtual void HandleDamage(int damageAmount)
    {
        _health.ChangeHealth(-damageAmount);

        if (!_health.HasHealth)
        {
            Die();
            return;
        }

        _fxHolder.AddDamagedVfx(_cachedTransform);
        _fxHolder.AddDamagedSfx();
    }

    public virtual void Deactivate()
    {
        enabled = false;
    }
    
    protected virtual void Die()
    {
        PlayDeathAnimation();
        _fxHolder.AddDeathSfx();

        _bodyCollider.enabled = false;
        _spriteRenderer.sortingOrder = 0;
        _health.HideHealthBar();
    }
    
    private void PlayDeathAnimation()
    {
        _animator.SetTrigger(UnitAnimationIdHelper.GetId(UnitAnimationState.Death));
    }
}
