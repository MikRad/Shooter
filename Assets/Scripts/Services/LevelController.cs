using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Player _player;
    
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
        EventBus.Get.Subscribe<PlayerCreatedEvent>(HandlePlayerCreated);
        EventBus.Get.Subscribe<PlayerDiedEvent>(HandlePlayerDied);
        EventBus.Get.Subscribe<EnemyCreatedEvent>(HandleEnemyCreated);
        
        EventBus.Get.Subscribe<EnemyBossActivationEvent>(HandleBossActivation);
        EventBus.Get.Subscribe<EnemyBossDiedEvent>(HandleBossDeath);
    }

    private void RemoveEventHandlers()
    {
        EventBus.Get.Unsubscribe<PlayerCreatedEvent>(HandlePlayerCreated);
        EventBus.Get.Unsubscribe<PlayerDiedEvent>(HandlePlayerDied);
        EventBus.Get.Unsubscribe<EnemyCreatedEvent>(HandleEnemyCreated);

        EventBus.Get.Unsubscribe<EnemyBossActivationEvent>(HandleBossActivation);
        EventBus.Get.Unsubscribe<EnemyBossDiedEvent>(HandleBossDeath);
    }

    private void HandlePlayerCreated(ref PlayerCreatedEvent ev)
    {
        _player = ev.Player;
        _player.Init(_diContainer);
        
        _diContainer.RegisterInstance(_player);
    }
    
    private void HandlePlayerDied()
    {
        LevelFailedEvent ev = new LevelFailedEvent();
        EventBus.Get.RaiseEvent(this, ref ev);
    }
    
    private void HandleEnemyCreated(ref EnemyCreatedEvent ev)
    {
        EnemyUnit enemy = ev.Enemy;
        
        enemy.Init(_diContainer);
        _enemyList.AddLast(enemy);
    }

    private void InitUIStats()
    {
        _uiViewsController.ResetPlayerUIStats();
        _uiViewsController.ResetBossUIStats();
        _uiViewsController.ShowUIView(UIViewType.PlayerUIStats);
        _uiViewsController.HideUIView(UIViewType.BossUIStats);
    }
    
    private void HandleBossActivation(ref EnemyBossActivationEvent ev)
    {
        if (ev.IsActivated)
        {
            _uiViewsController.ShowUIView(UIViewType.BossUIStats);
        }
        else
        {
            _uiViewsController.HideUIView(UIViewType.BossUIStats);
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
}
