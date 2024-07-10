using System.Collections.Generic;
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
    private EnemyFactory _enemyFactory;
    
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
        _enemyFactory = _diContainer.Resolve<EnemyFactory>();
        _uiViewsController = _diContainer.Resolve<UIViewsController>();

        CreateUnits();
        
        InitUIStats();
    }

    private void AddEventHandlers()
    {
        EventBus.Get.Subscribe<PlayerDiedEvent>(HandlePlayerDied);
        
        EventBus.Get.Subscribe<EnemyBossActivationEvent>(HandleBossActivation);
        EventBus.Get.Subscribe<EnemyBossDiedEvent>(HandleBossDeath);
    }

    private void RemoveEventHandlers()
    {
        EventBus.Get.Unsubscribe<PlayerDiedEvent>(HandlePlayerDied);

        EventBus.Get.Unsubscribe<EnemyBossActivationEvent>(HandleBossActivation);
        EventBus.Get.Unsubscribe<EnemyBossDiedEvent>(HandleBossDeath);
    }

    private void CreateUnits()
    {
        EnemyPatrolPoint[] enemyPatrolPoints = FindObjectsOfType<EnemyPatrolPoint>();
        EnemyStartPointData[] enemyStartPointDatas = FindObjectsOfType<EnemyStartPointData>();
        PlayerStartPoint playerStartPoint = FindObjectOfType<PlayerStartPoint>();
        
        _player = _playerFactory.CreatePlayer(playerStartPoint.transform.position);
        _cmCamera.Follow = _player.transform;

        _enemyFactory.InitAllPatrolPositions(enemyPatrolPoints);
        
        foreach (EnemyStartPointData data in enemyStartPointDatas)
        {
            EnemyUnit enemy = _enemyFactory.CreateEnemy(data);
            _enemyList.AddLast(enemy);

            Destroy(data.gameObject);
        }

        foreach (EnemyPatrolPoint point in enemyPatrolPoints)
        {
            Destroy(point.gameObject);
        }
        
        Destroy(playerStartPoint.gameObject);
    }

    private void HandlePlayerDied()
    {
        EventBus.Get.RaiseEvent(this, new LevelFailedEvent());
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
