using UnityEngine;

[RequireComponent(typeof(PlayerMovement)), RequireComponent(typeof(PlayerShooting))]
public class Player : BaseUnit
{
    private IPlayerInput _playerInput;
    private PlayerMovement _movement;
    private PlayerShooting _shooting;

    private float _timeToNextAttack;

    public Transform Transform => _bodyTransform;

    protected override void Awake()
    {
        base.Awake();
        
        _movement = GetComponent<PlayerMovement>();
        _shooting = GetComponent<PlayerShooting>();
    }

    protected override void Start()
    {
        base.Start();

        PlayerCreatedEvent ev = new PlayerCreatedEvent(this);
        EventBus.Get.RaiseEvent(this, ref ev);
    }

    private void Update()
    {
        UpdateAttackDelay();

        CheckAttackPossibility();
    }

    public override void Init(DIContainer diContainer)
    {
        base.Init(diContainer);
        
        _playerInput = diContainer.Resolve<IPlayerInput>();
        BulletSpawner bulletSpawner = diContainer.Resolve<BulletSpawner>();
        _movement.Init(_playerInput);
        _shooting.Init(_playerInput, bulletSpawner, _fxHolder);
    }
    
    public override void Deactivate()
    {
        base.Deactivate();
        
        _movement.Stop();
        _movement.enabled = false;
        _shooting.enabled = false;
    }

    public override void HandleDamage(int damageAmount)
    {
        base.HandleDamage(damageAmount);

        PlayerHealthChangedEvent ev = new PlayerHealthChangedEvent(_health.HealthFullness);
        EventBus.Get.RaiseEvent(this, ref ev);
    }

    public bool TryCollectHealth(HealthItem healthItem)
    {
        if (_health.HasMaxHealth)
            return false;
        
        _health.ChangeHealth(healthItem.HealthAmount);
        _fxHolder.PlayHealthCollectSfx();
        
        PlayerHealthChangedEvent ev = new PlayerHealthChangedEvent(_health.HealthFullness);
        EventBus.Get.RaiseEvent(this, ref ev);
        
        return true;
    }

    public bool TryCollectAmmo(GunMagazineItem gunMagazine)
    {
        if (_shooting.HasMaxAmmo)
            return false;

        _shooting.AddAmmo(gunMagazine.AmmoAmount);
        _fxHolder.PlayGunMagazineCollectSfx();
        
        PlayerAmmoChangedEvent ev = new PlayerAmmoChangedEvent(_shooting.AmmoFullness);
        EventBus.Get.RaiseEvent(this, ref ev);

        return true;
    }
    
    private void CheckAttackPossibility()
    {
        if (IsDead)
            return;

        if (CanAttack() && _playerInput.IsFirePressed)
        {
            Attack();
        }
    }

    private void UpdateAttackDelay()
    {
        if (_timeToNextAttack > 0)
        {
            _timeToNextAttack -= Time.deltaTime;
        }
    }

    private bool CanAttack()
    {
        return _timeToNextAttack <= 0;
    }

    private void Attack()
    {
        _timeToNextAttack = _attackDelay;
        
        if(_shooting.HasAmmo)
        {
            _shooting.Shoot();
            
            PlayerAmmoChangedEvent ev = new PlayerAmmoChangedEvent(_shooting.AmmoFullness);
            EventBus.Get.RaiseEvent(this, ref ev);
        }
        else
        {
            _fxHolder.PlayNoAmmoSfx();
        }
    }
    
    protected override void Die()
    {
        base.Die();

        _movement.Stop();
        _movement.enabled = false;
        _shooting.Deactivate();
        _shooting.enabled = false;
        
        PlayerDiedEvent ev = new PlayerDiedEvent();
        EventBus.Get.RaiseEvent(this, ref ev);
    }
}