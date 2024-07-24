using Units.Player;
using UnityEngine;

namespace PickupItems
{
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
}