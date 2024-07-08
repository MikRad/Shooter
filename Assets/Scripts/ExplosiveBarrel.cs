using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(DamageDealer))]
[RequireComponent(typeof(Collider2D))]
public class ExplosiveBarrel : MonoBehaviour, IDamageable, IExplosive
{
    [Header("Base Settings")]
    [SerializeField] private float _damageRadius = 2f;
    [SerializeField] private float _healthAmount = 5f;
    [SerializeField] private float _chainExplosionDelay = 0.25f;
    [SerializeField] private LayerMask _damageMask;

    [Header("Explosion Vfx")]
    [SerializeField] private VfxType _vfxType;

    [Header("Explosion Sfx")]
    [SerializeField] private SfxType[] _explosionSfxTypes;
    
    private DamageDealer _damageDealer;
    private Transform _cachedTransform;
    
    private void Awake()
    {
        _cachedTransform = transform;
        _damageDealer = GetComponent<DamageDealer>();
    }

    public void HandleDamage(int damageAmount)
    {
        _healthAmount -= damageAmount;
        if (_healthAmount <= 0)
        {
            Explode();
        }
    }
    
    public void HandleExplosiveDamage(int damageAmount)
    {
        StartCoroutine(UpdateChainExplosionDelay(damageAmount));
    }
    
    private void Explode()
    {
        Collider2D[] objectsInRadius = Physics2D.OverlapCircleAll(_cachedTransform.position, _damageRadius, _damageMask);

        foreach (Collider2D obj in objectsInRadius)
        {
            if (obj.TryGetComponent(out IDamageable damageable))
            {
                if (damageable is IExplosive explosive)
                    explosive.HandleExplosiveDamage(_damageDealer.DamageAmount);
                else
                    damageable.HandleDamage(_damageDealer.DamageAmount);
            }
        }
        
        AddExplosionVfx();
        PlayExplosionSfx();
        
        Destroy(gameObject);
    }
    
    private void AddExplosionVfx()
    {
        EventBus.Get.RaiseEvent(this, new VfxNeededEvent(_vfxType, _cachedTransform));
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
    
    private IEnumerator UpdateChainExplosionDelay(int damageAmount)
    {
        yield return new WaitForSeconds(_chainExplosionDelay);

        HandleDamage(damageAmount);
    }

    private void PlayExplosionSfx()
    {
        int rndIdx = Random.Range(0, _explosionSfxTypes.Length);
        
        EventBus.Get.RaiseEvent(this, new SfxNeededEvent(_explosionSfxTypes[rndIdx]));
    }
}
