using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Player _player;
    
    private IBossConditionChecker _bossConditionChecker;
    private readonly LinkedList<EnemyUnit> _enemyList = new LinkedList<EnemyUnit>();
    
    private UIViewsController _uiViewsController;
    private DIContainer _diContainer;
    
    private void Awake()
    {
        AddEventHandlers();
    }

    private void OnDestroy()
    {
        RemoveEventHandlers();
    }

    public void Init(DIContainer diContainer)
    {
        _diContainer = diContainer;
        
        _uiViewsController = _diContainer.Resolve<UIViewsController>();
        
        InitUIStats();
    }
    
    private void AddEventHandlers()
    {
        EventBus.Get.Subscribe<EnemyCreatedEvent>(HandledEnemyCreated);
        EventBus.Get.Subscribe<PlayerCreatedEvent>(HandledPlayerCreated);
        EventBus.Get.Subscribe<PlayerDiedEvent>(HandledPlayerDied);
        EventBus.Get.Subscribe<PlayerHealthChangedEvent>(HandledPlayerHealthChanged);
        EventBus.Get.Subscribe<PlayerAmmoChangedEvent>(HandledPlayerAmmoChanged);
        
        ExplosiveBarrel.OnCreated += HandleExplosiveBarrelCreated;
    }

    private void RemoveEventHandlers()
    {
        EventBus.Get.Unsubscribe<EnemyCreatedEvent>(HandledEnemyCreated);
        EventBus.Get.Unsubscribe<PlayerCreatedEvent>(HandledPlayerCreated);
        EventBus.Get.Unsubscribe<PlayerDiedEvent>(HandledPlayerDied);
        EventBus.Get.Unsubscribe<PlayerHealthChangedEvent>(HandledPlayerHealthChanged);
        EventBus.Get.Unsubscribe<PlayerAmmoChangedEvent>(HandledPlayerAmmoChanged);
        
        ExplosiveBarrel.OnCreated -= HandleExplosiveBarrelCreated;

        RemoveBossConditionHandlers();
    }

    private void AddBossConditionHandlers()
    {
        _bossConditionChecker.OnDied += HandleBossDeath;
        _bossConditionChecker.OnHealthChanged += HandleBossHealthChange;
        _bossConditionChecker.OnActivated += HandleBossActivation;
    }
    
    private void RemoveBossConditionHandlers()
    {
        _bossConditionChecker.OnDied -= HandleBossDeath;
        _bossConditionChecker.OnHealthChanged -= HandleBossHealthChange;
        _bossConditionChecker.OnActivated -= HandleBossActivation;
    }
    
    private void HandledPlayerCreated(ref PlayerCreatedEvent ev)
    {
        _player = ev.Player;
        _player.Init(_diContainer);
        
        _diContainer.RegisterInstance(_player);
    }
    
    private void HandledPlayerDied(ref PlayerDiedEvent raisedevent)
    {
        LevelFailedEvent ev = new LevelFailedEvent();
        EventBus.Get.RaiseEvent(this, ref ev);
    }
    
    private void HandledPlayerHealthChanged(ref PlayerHealthChangedEvent ev)
    {
        _uiViewsController.SetPlayerUIHealthFullness(ev.HealthFullness);
    }
    
    private void HandledPlayerAmmoChanged(ref PlayerAmmoChangedEvent ev)
    {
        _uiViewsController.SetPlayerUIAmmoFullness(ev.AmmoFullness);
    }

    
    private void HandledEnemyCreated(ref EnemyCreatedEvent ev)
    {
        EnemyUnit enemy = ev.Enemy;
        
        enemy.Init(_diContainer);
        _enemyList.AddLast(enemy);
        
        if(enemy is IBossConditionChecker bossConditionChecker)
        {
            _bossConditionChecker = bossConditionChecker;

            AddBossConditionHandlers();
        }
    }

    private void HandleBossDeath()
    {
        foreach (EnemyUnit enemy in _enemyList)
        {
            enemy.Deactivate();
        }
        
        _player.Deactivate();
        
        LevelCompletedEvent ev = new LevelCompletedEvent();
        EventBus.Get.RaiseEvent(this, ref ev);
    }

    private void HandleExplosiveBarrelCreated(ExplosiveBarrel barrel)
    {
        barrel.Init(_diContainer);
    }
    
    private void InitUIStats()
    {
        _uiViewsController.ResetPlayerUIStats();
        _uiViewsController.ResetBossUIStats();
        _uiViewsController.ShowUIView(UIViewType.PlayerUIStats);
        _uiViewsController.HideUIView(UIViewType.BossUIStats);
    }
    
    private void HandleBossActivation(bool isActivated)
    {
        if (isActivated)
        {
            _uiViewsController.ShowUIView(UIViewType.BossUIStats);
        }
        else
        {
            _uiViewsController.HideUIView(UIViewType.BossUIStats);
        }
    }

    private void HandleBossHealthChange(float value)
    {
        _uiViewsController.SetBossUIHealthFullness(value);
    }
}
