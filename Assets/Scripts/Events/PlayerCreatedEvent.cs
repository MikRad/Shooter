public struct PlayerCreatedEvent : IEvent
{
    public Player Player { get; private set; }
    
    public PlayerCreatedEvent(Player player)
    {
        Player = player;
    }
}
