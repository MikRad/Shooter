using UnityEngine;

public class GunMagazineItem : BasePickupItem
{
    [SerializeField] private int _ammoAmount = 50;

    public int AmmoAmount => _ammoAmount;

    protected override void HandleCollecting(Player player)
    {
        if (player.TryCollectAmmo(this))
        {
            Remove();
        }
    }
}