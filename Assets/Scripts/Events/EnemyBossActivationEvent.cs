namespace Events.Services
{
    public struct EnemyBossActivationEvent : IEvent
    {
        public bool IsActivated { get; private set; }
    
        public EnemyBossActivationEvent(bool isActivated)
        {
            IsActivated = isActivated;
        }
    }
}