namespace Events.Services
{
    public struct PlayerHealthChangedEvent : IEvent
    {
        public float HealthFullness { get; private set; }

        public PlayerHealthChangedEvent(float healthFullness)
        {
            HealthFullness = healthFullness;
        }
    }
}
