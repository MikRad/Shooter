﻿public struct GameOverPanelClosedEvent : IEvent
{
    public GameOverPanel.UserAction Action { get; private set; }

    public GameOverPanelClosedEvent(GameOverPanel.UserAction action)
    {
        Action = action;
    }
}

