public struct GameCompletedPanelClosedEvent : IEvent
{
    public GameCompletedPanel.UserAction Action { get; private set; }
    
    public GameCompletedPanelClosedEvent(GameCompletedPanel.UserAction action)
    {
        Action = action;
    }
}
