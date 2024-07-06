using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement)), RequireComponent(typeof(PlayerShooting))]
public class Player : BaseUnit
{
    private PlayerMovement _movement;
    private PlayerShooting _shooting;

    private float _timeToNextAttack;

    public Transform Transform => _bodyTransform;

    public event Action OnDied;
    public event Action<float> OnHealthChanged;
    public event Action<float> OnAmmoChanged;

    protected override void Awake()
    {
        base.Awake();
        
        _movement = GetComponent<PlayerMovement>();
        _shooting = GetComponent<PlayerShooting>();
    }

    private void Update()
    {
        UpdateAttackDelay();

        CheckFireButton();
    }

    public override void Init(DIContainer diContainer)
    {
        base.Init(diContainer);
        
        BulletSpawner bulletSpawner = diContainer.Resolve<BulletSpawner>();
        _shooting.Init(bulletSpawner, _audioController);
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

        OnHealthChanged?.Invoke(HealthFullness);
    }

    public bool TryCollectHealth(HealthItem healthItem)
    {
        if (HasMaxHealth)
            return false;
        
        AddHealth(healthItem.HealthAmount);
        _audioController.PlaySfx(SfxType.HealthCollect);    
        
        OnHealthChanged?.Invoke(HealthFullness);
        
        return true;
    }

    public bool TryCollectAmmo(GunMagazineItem gunMagazine)
    {
        if (_shooting.HasMaxAmmo)
            return false;

        _shooting.AddAmmo(gunMagazine.AmmoAmount);
        _audioController.PlaySfx(SfxType.GunMagazineCollect);    
        
        OnAmmoChanged?.Invoke(_shooting.AmmoFullness);

        return true;
    }
    
    private void CheckFireButton()
    {
        if (IsDead)
            return;

        if (CanAttack() && Input.GetButton("Fire1"))
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
            
            OnAmmoChanged?.Invoke(_shooting.AmmoFullness);
        }
        else
        {
            _audioController.PlaySfx(SfxType.NoAmmo);
        }
    }
    
    protected override void Die()
    {
        base.Die();

        _movement.Stop();
        _movement.enabled = false;
        
        OnDied?.Invoke();
    }
}