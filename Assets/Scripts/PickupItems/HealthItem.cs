using UnityEngine;

public class HealthItem : BasePickupItem
{
    [SerializeField] private int _healthAmount = 10;

    public int HealthAmount => _healthAmount;

    protected override void HandleCollecting(Player player)
    {
        if (player.TryCollectHealth(this))
        {
            Remove();
        }
    }
}
