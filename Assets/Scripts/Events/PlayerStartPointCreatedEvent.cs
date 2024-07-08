using UnityEngine;

public struct PlayerStartPointCreatedEvent : IEvent
{
    public Vector3 Position { get; private set; }
    // public PlayerStartPoint Target { get; private set; }
    
    public PlayerStartPointCreatedEvent(Vector3 position)
    {
        Position = position;
        // Target = target;
    }
}
