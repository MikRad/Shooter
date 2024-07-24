using Audio;
using UnityEngine;
using Events.Services;
using Events.Services.Fx;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int _damageAmount;

    public int DamageAmount => _damageAmount;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.HandleDamage(_damageAmount);
        }
        else
        {
            EventBus.Get.RaiseEvent(this, new SfxNeededEvent(SfxType.ObstacleHit));
        }
    }
}