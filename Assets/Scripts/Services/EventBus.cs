﻿using System;
using System.Collections.Generic;

public class EventBus
{
    private readonly Dictionary<Type, object> _bindingsMap = new Dictionary<Type, object>();
    private static EventBus _instance;

    public static EventBus Get => _instance ??= new EventBus();

    public void Subscribe<TEvent>(EventCallback<TEvent> callback, int priority = 0) where TEvent : IEvent
    {
        EventBinding<TEvent> binding = GetBinding<TEvent>();

        if (binding == null)
        {
            binding = new EventBinding<TEvent>();
            _bindingsMap[typeof(TEvent)] = binding;
        }
        
        binding.Add(callback, priority);
    }
    
    public void Subscribe<TEvent>(Action callback, int priority = 0) where TEvent : IEvent
    {
        EventBinding<TEvent> binding = GetBinding<TEvent>();

        if (binding == null)
        {
            binding = new EventBinding<TEvent>();
            _bindingsMap[typeof(TEvent)] = binding;
        }
        
        binding.Add(callback, priority);
    }
    
    public void Unsubscribe<TEvent>(EventCallback<TEvent> callback) where TEvent : IEvent
    {
        EventBinding<TEvent> binding = GetBinding<TEvent>();

        binding?.Remove(callback);
    }

    public void Unsubscribe<TEvent>(Action callback) where TEvent : IEvent
    {
        EventBinding<TEvent> binding = GetBinding<TEvent>();

        binding?.Remove(callback);
    }
    
    public void RaiseEvent<TEvent>(object sender, ref TEvent ev) where TEvent : IEvent
    {
        EventBinding<TEvent> binding = GetBinding(ref ev);
        
        binding?.Raise(ref ev);
    }

    private EventBinding<TEvent> GetBinding<TEvent>() where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);
        
        if (_bindingsMap.TryGetValue(eventType, out object bindingObj))
            return bindingObj as EventBinding<TEvent>;

        return null;
    }
    
    private EventBinding<TEvent> GetBinding<TEvent>(ref TEvent ev) where TEvent : IEvent
    {
        Type eventType = ev.GetType();
        
        if (_bindingsMap.TryGetValue(eventType, out object bindingObj))
            return bindingObj as EventBinding<TEvent>;

        return null;
    }
    
    private EventBus() { }
}