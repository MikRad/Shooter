using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Player _player;
    
    private IBossConditionChecker _bossConditionChecker;
    private readonly LinkedList<EnemyUnit> _enemyList = new LinkedList<EnemyUnit>();
    
    private UIViewsController _uiViewsController;
    private DiContainer _diContainer;
    
    public event Action OnLevelCompleted; 
    public event Action OnLevelFailed; 

    private void Awake()
    {
        EnemyUnit.OnCreated += HandledEnemyCreated;
        ExplosiveBarrel.OnCreated += HandleExplosiveBarrelCreated;
        
        _player.OnDied += HandlePlayerDeath;
        _player.OnHealthChanged += HandlePlayerHealthChange;
        _player.OnAmmoChanged += HandlePlayerAmmoChange;
    }

    private void OnDestroy()
    {
        RemoveEventHandlers();
    }

    public void Init(DiContainer diContainer)
    {
        _diContainer = diContainer;
        _diContainer.Register(_player);
        
        _player.Init(diContainer);
        
        _uiViewsController = _diContainer.Resolve<UIViewsController>();
        
        InitUIStats();
    }
    
    private void RemoveEventHandlers()
    {
        EnemyUnit.OnCreated -= HandledEnemyCreated;
        ExplosiveBarrel.OnCreated -= HandleExplosiveBarrelCreated;

        _player.OnDied -= HandlePlayerDeath;
        _player.OnHealthChanged -= HandlePlayerHealthChange;
        _player.OnAmmoChanged -= HandlePlayerAmmoChange;

        _bossConditionChecker.OnDied -= HandleBossDeath;
        _bossConditionChecker.OnHealthChanged -= HandleBossHealthChange;
        _bossConditionChecker.OnActivated -= HandleBossActivation;
    }
    
    private void HandledEnemyCreated(EnemyUnit enemy)
    {
        enemy.Init(_diContainer);
        _enemyList.AddLast(enemy);
        
        if(enemy is IBossConditionChecker bossConditionChecker)
        {
            _bossConditionChecker = bossConditionChecker;
        
            _bossConditionChecker.OnDied += HandleBossDeath;
            _bossConditionChecker.OnHealthChanged += HandleBossHealthChange;
            _bossConditionChecker.OnActivated += HandleBossActivation;
        }
    }
    
    private void HandleBossDeath()
    {
        foreach (EnemyUnit enemy in _enemyList)
        {
            enemy.Deactivate();
        }
        
        _player.Deactivate();
        
        OnLevelCompleted?.Invoke();
    }

    private void HandlePlayerDeath()
    {
        OnLevelFailed?.Invoke();
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

    private void HandlePlayerHealthChange(float value)
    {
        _uiViewsController.SetPlayerUIHealthFullness(value);
    }
    
    private void HandleBossHealthChange(float value)
    {
        _uiViewsController.SetBossUIHealthFullness(value);
    }

    private void HandlePlayerAmmoChange(float value)
    {
        _uiViewsController.SetPlayerUIAmmoFullness(value);
    }
}
