using UnityEngine;

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
    }
}