using UnityEngine;

public class PlayerUIStats : UIView
{
    [Header("UI elements")]
    [SerializeField] private UIProgressBar _healthBar;
    [SerializeField] private UIProgressBar _ammoBar;
    
    public override void Show()
    {
        SetActive(true);

        _tweener.Show();
    }

    public void SetHealthFullness(float healthFullness)
    {
        _healthBar.SetValue(healthFullness);
    }
    
    public void SetAmmoFullness(float ammoFullness)
    {
        _ammoBar.SetValue(ammoFullness);
    }
}
