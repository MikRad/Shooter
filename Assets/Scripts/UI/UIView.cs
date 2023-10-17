﻿using System;
using UnityEngine;

[RequireComponent(typeof(UITweener))]
public abstract class UIView : MonoBehaviour
{
    protected UITweener _tweener;
    
    public event Action<UIEventType, object> OnUserEvent;

    protected virtual void Awake()
    {
        _tweener = GetComponent<UITweener>();
        
        AddElementsListeners();
        SetEnableElements(false);
        SetActive(false);
    }

    protected virtual void OnDestroy()
    { 
        RemoveElementsListeners(); 
    }

    public virtual void Show()
    {
        SetActive(true);
        
        _tweener.Show((() => SetEnableElements(true)));
    }

    public virtual void Hide()
    {
        SetEnableElements(false);
        
        _tweener.Hide(HandleHideCompleted);
    }

    protected abstract void AddElementsListeners();
    protected abstract void RemoveElementsListeners();
    protected abstract void SetEnableElements(bool isEnabled);
    protected void SetActive(bool isActive) => gameObject.SetActive(isActive);

    protected virtual void HandleHideCompleted()
    {
        SetActive(false);
    }

    protected void InvokeOnUserEvent(UIEventType eventType, object param)
    {
        OnUserEvent?.Invoke(eventType, param);
    }
}