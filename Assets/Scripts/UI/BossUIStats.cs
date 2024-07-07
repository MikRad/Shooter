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
}
