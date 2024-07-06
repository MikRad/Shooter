using UnityEngine;

public class BaseShooting : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] protected Transform _bodyTransform;
    
    [Header("Shooting")]
    [SerializeField] protected BulletType _bulletType;
    [SerializeField] protected Transform _bulletSpawnPoint;

    protected Animator _animator;
    protected BulletSpawner _bulletSpawner;

    protected UnitFxHolder _fxHolder; 

    public Vector3 BulletSpawnPosition => _bulletSpawnPoint.position;

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void Init(BulletSpawner bulletSpawner, UnitFxHolder fxHolder)
    {
        _bulletSpawner = bulletSpawner;
        _fxHolder = fxHolder;
    }
    
    public virtual void Shoot()
    {
        CreateBullet();
        PlayShootAnimation();
    }

    public void ShootAt(in Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - _bulletSpawnPoint.position;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction);
        
        CreateBullet(rotation);
        PlayShootAnimation();
    }
    
    private void CreateBullet()
    {
        _bulletSpawner.SpawnBullet(_bulletType, _bulletSpawnPoint.position, _bodyTransform.rotation);
        PlayShotSfx();
    }
    
    private void CreateBullet(Quaternion rotation)
    {
        _bulletSpawner.SpawnBullet(_bulletType, _bulletSpawnPoint.position, rotation);
        PlayShotSfx();
    }

    private void PlayShootAnimation()
    {
        _animator.SetTrigger(UnitAnimationIdHelper.GetId(UnitAnimationState.Attack));
    }

    private void PlayShotSfx()
    {
        _fxHolder.PlayShootSfx();
    }
}