﻿using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cmCamera;

    private Player _player;
    private readonly LinkedList<EnemyUnit> _enemyList = new LinkedList<EnemyUnit>();
    
    private UIViewsController _uiViewsController;
    private DIContainer _diContainer;
    private PlayerFactory _playerFactory;
    
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

        _playerFactory = _diContainer.Resolve<PlayerFactory>();
        _uiViewsController = _diContainer.Resolve<UIViewsController>();
        
        InitUIStats();
    }
    
    private void AddEventHandlers()
    {
        EventBus.Get.Subscribe<PlayerStartPointCreatedEvent>(HandlePlayerStartPointCreated);
        EventBus.Get.Subscribe<PlayerCreatedEvent>(HandlePlayerCreated);
        EventBus.Get.Subscribe<PlayerDiedEvent>(HandlePlayerDied);
        EventBus.Get.Subscribe<EnemyCreatedEvent>(HandleEnemyCreated);
        
        EventBus.Get.Subscribe<EnemyBossActivationEvent>(HandleBossActivation);
        EventBus.Get.Subscribe<EnemyBossDiedEvent>(HandleBossDeath);
    }

    private void RemoveEventHandlers()
    {
        EventBus.Get.Unsubscribe<PlayerStartPointCreatedEvent>(HandlePlayerStartPointCreated);
        EventBus.Get.Unsubscribe<PlayerCreatedEvent>(HandlePlayerCreated);
        EventBus.Get.Unsubscribe<PlayerDiedEvent>(HandlePlayerDied);
        EventBus.Get.Unsubscribe<EnemyCreatedEvent>(HandleEnemyCreated);

        EventBus.Get.Unsubscribe<EnemyBossActivationEvent>(HandleBossActivation);
        EventBus.Get.Unsubscribe<EnemyBossDiedEvent>(HandleBossDeath);
    }

    private void HandlePlayerStartPointCreated(PlayerStartPointCreatedEvent ev)
    {
        _playerFactory.CreatePlayer(ev.Position);        
    }
    
    private void HandlePlayerCreated(PlayerCreatedEvent ev)
    {
        _player = ev.Player;
        _cmCamera.Follow = _player.transform;
        // _player.Init(_diContainer);
    }
    
    private void HandlePlayerDied()
    {
        EventBus.Get.RaiseEvent(this, new LevelFailedEvent());
    }
    
    private void HandleEnemyCreated(EnemyCreatedEvent ev)
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
    
    private void HandleBossActivation(EnemyBossActivationEvent ev)
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
        
        EventBus.Get.RaiseEvent(this, new LevelCompletedEvent());
    }
}
