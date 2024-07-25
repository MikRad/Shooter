namespace Events
{
    public struct PlayerAmmoChangedEvent : IEvent
    {
        public float AmmoFullness { get; private set; }
    
        public PlayerAmmoChangedEvent(float ammoFullness)
        {
            AmmoFullness = ammoFullness;
        }
    }
}
