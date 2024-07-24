using Units.Abstractions;

namespace Events.Services
{
    public struct EnemyCreatedEvent : IEvent
    {
        public EnemyUnit Enemy { get; private set; }
    
        public EnemyCreatedEvent(EnemyUnit enemy)
        {
            Enemy = enemy;
        }
    }
}