using UnityEngine;

public class BossUIStats : UIView
{
    [Header("UI elements")]
    [SerializeField] private UIProgressBar _healthBar;
    
    public override void Show()
    {
        SetActive(true);

        _tweener.Show();
    }

    public void SetHealthFullness(float healthFullness)
    {
        _healthBar.SetValue(healthFullness);
    }
    
    protected override void AddElementsListeners()
    {
    }

    protected override void RemoveElementsListeners()
    {
    }

    protected override void SetEnableElements(bool isEnabled)
    {
    }
}
