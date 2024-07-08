using UnityEngine;

public class BossUIStats : UIView
{
    [Header("UI elements")]
    [SerializeField] private UIProgressBar _healthBar;

    protected override void Awake()
    {
        base.Awake();
        
        EventBus.Get.Subscribe<EnemyBossHealthChangedEvent>(HandleEnemyBossHealthChanged);
    }

    private void OnDestroy()
    {
        EventBus.Get.Unsubscribe<EnemyBossHealthChangedEvent>(HandleEnemyBossHealthChanged);
    }

    public override void Show()
    {
        SetActive(true);

        _tweener.Show();
    }

    public void Reset()
    {
        _healthBar.SetValue(1f);
    }
    
    private void HandleEnemyBossHealthChanged(EnemyBossHealthChangedEvent ev)
    {
        _healthBar.SetValue(ev.HealthFullness);
    }
}
