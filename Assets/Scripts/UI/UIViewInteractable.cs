public abstract class UIViewInteractable : UIView
{
    protected override void Awake()
    {
        base.Awake();
        
        AddElementsListeners();
        SetEnableElements(false);
    }

    protected virtual void OnDestroy()
    { 
        RemoveElementsListeners(); 
    }
    
    public override void Show()
    {
        SetActive(true);
        
        _tweener.Show((() => SetEnableElements(true)));
    }

    public override void Hide()
    {
        base.Hide();
        
        SetEnableElements(false);
    }

    protected abstract void AddElementsListeners();
    protected abstract void RemoveElementsListeners();
    protected abstract void SetEnableElements(bool isEnabled);
}
