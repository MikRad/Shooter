namespace Events
{
    public struct EnemyBossHealthChangedEvent : IEvent
    {
        public float HealthFullness { get; private set; }
    
        public EnemyBossHealthChangedEvent(float healthFullness)
        {
            HealthFullness = healthFullness;
        }
    }
}
