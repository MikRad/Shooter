using System;
using UnityEngine;

[RequireComponent(typeof(UITweener))]
public abstract class UIView : MonoBehaviour
{
    protected UITweener _tweener;
    
    public event Action<UIEventType, object> OnUserEvent;

    protected virtual void Awake()
    {
        _tweener = GetComponent<UITweener>();
        
        SetActive(false);
    }

    public abstract void Show();
    
    public virtual void Hide()
    {
        _tweener.Hide(HandleHideCompleted);
    }
    
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