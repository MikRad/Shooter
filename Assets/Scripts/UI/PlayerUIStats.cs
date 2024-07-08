using UnityEngine;

public class PlayerUIStats : UIView
{
    [Header("UI elements")]
    [SerializeField] private UIProgressBar _healthBar;
    [SerializeField] private UIProgressBar _ammoBar;


    protected override void Awake()
    {
        base.Awake();
        
        EventBus.Get.Subscribe<PlayerHealthChangedEvent>(HandlePlayerHealthChanged);
        EventBus.Get.Subscribe<PlayerAmmoChangedEvent>(HandlePlayerAmmoChanged);
    }

    private void OnDestroy()
    {
        EventBus.Get.Unsubscribe<PlayerHealthChangedEvent>(HandlePlayerHealthChanged);
        EventBus.Get.Unsubscribe<PlayerAmmoChangedEvent>(HandlePlayerAmmoChanged);
    }

    public override void Show()
    {
        SetActive(true);

        _tweener.Show();
    }

    public void Reset()
    {
        _healthBar.SetValue(1f);
        _ammoBar.SetValue(1f);
    }
    
    private void HandlePlayerHealthChanged(PlayerHealthChangedEvent ev)
    {
        _healthBar.SetValue(ev.HealthFullness);
    }
    
    private void HandlePlayerAmmoChanged(PlayerAmmoChangedEvent ev)
    {
        _ammoBar.SetValue(ev.AmmoFullness);
    }
}
