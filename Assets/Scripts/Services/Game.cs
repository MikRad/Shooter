using System.Collections;
using Audio.Services;
using DI.Services;
using Events;
using Events.Services;
using Events.UI;
using Factories;
using Factories.UI;
using Input;
using UI;
using UnityEngine;
using Vfx.Services;

namespace Services
{
    public class Game : MonoBehaviour
    {
        [Header("Panel delays")]
        [SerializeField] private float _gameOverPanelDelay = 2f;
        [SerializeField] private float _nextLevelPanelDelay = 1f;

        private UIViewsController _uiViewsController;
        private LevelController _levelController;
        private AudioController _audioController;
        private BulletSpawner _bulletSpawner;
        private PickupItemSpawner _pickupItemSpawner;
        private VfxSpawner _vfxSpawner;
        private SceneLoader _sceneLoader;
        private DIContainer _diContainer;

        private int _levelNumber;

        private void OnDestroy()
        {
            RemoveEventHandlers();
        }
    
        public void Init()
        {
            FindServices();
            RegisterServices();
            InitServices();
            AddEventHandlers();
        
            _levelNumber = 1;
        
            _uiViewsController.ShowUIView(UIViewType.LevelLoadProgress);
            _sceneLoader.LoadLevel(_levelNumber, OnLevelLoadProgress, OnLevelLoadCompleted);
        }

        private void FindServices()
        {
            _uiViewsController = GetComponentInChildren<UIViewsController>();
            _levelController = GetComponentInChildren<LevelController>();
            _audioController = GetComponentInChildren<AudioController>();
            _bulletSpawner = GetComponentInChildren<BulletSpawner>();
            _pickupItemSpawner = GetComponentInChildren<PickupItemSpawner>();
            _vfxSpawner = GetComponentInChildren<VfxSpawner>();
            _sceneLoader = GetComponentInChildren<SceneLoader>();
        }

        private void RegisterServices()
        {
            _diContainer = new DIContainer();
            _diContainer.RegisterInstance(_uiViewsController);
            _diContainer.RegisterInstance(_levelController);
            _diContainer.RegisterInstance(_audioController);
            _diContainer.RegisterInstance(_bulletSpawner);
            _diContainer.RegisterInstance(_pickupItemSpawner);
            _diContainer.RegisterInstance(_vfxSpawner);
            _diContainer.RegisterSingleton<IPlayerInput>((_) => new DesktopPlayerInput());
            _diContainer.RegisterSingleton<IResourcesDataProvider>((_) => new ResourcesDataProvider());
            _diContainer.RegisterSingleton((c) => new PlayerFactory(c));
            _diContainer.RegisterSingleton((c) => new EnemyFactory(c));
            _diContainer.RegisterSingleton((c) => new UIViewFactory(c.Resolve<IResourcesDataProvider>()));
        }
    
        private void InitServices()
        {
            _uiViewsController.Init(_diContainer);
            _levelController.Init(_diContainer);
        }
    
        private void AddEventHandlers()
        {
            EventBus.Get.Subscribe<LevelCompletedPanelClosedEvent>(HandleLevelCompletedPanelClosed);
            EventBus.Get.Subscribe<GameOverPanelClosedEvent>(HandleGameOverPanelClosed);
            EventBus.Get.Subscribe<GameCompletedPanelClosedEvent>(HandleGameCompletedPanelClosed);
        
            EventBus.Get.Subscribe<LevelCompletedEvent>(HandleLevelCompleted);
            EventBus.Get.Subscribe<LevelFailedEvent>(HandleLevelFailed);
        }
    
        private void RemoveEventHandlers()
        {
            EventBus.Get.Unsubscribe<LevelCompletedPanelClosedEvent>(HandleLevelCompletedPanelClosed);
            EventBus.Get.Unsubscribe<GameOverPanelClosedEvent>(HandleGameOverPanelClosed);
            EventBus.Get.Unsubscribe<GameCompletedPanelClosedEvent>(HandleGameCompletedPanelClosed);
        
            EventBus.Get.Unsubscribe<LevelCompletedEvent>(HandleLevelCompleted);
            EventBus.Get.Unsubscribe<LevelFailedEvent>(HandleLevelFailed);
        }

        private void HandleLevelCompletedPanelClosed()
        {
            GoToNextLevel();
        }
    
        private void HandleGameOverPanelClosed(GameOverPanelClosedEvent ev)
        {
            switch (ev.Action)
            {
                case GameOverPanel.UserAction.PlayAgain:
                    RestartLevel();
                    break;
                case GameOverPanel.UserAction.Exit:
                    ExitGame();
                    break;
            }
        }

        private void HandleGameCompletedPanelClosed(GameCompletedPanelClosedEvent ev)
        {
            switch (ev.Action)
            {
                case GameCompletedPanel.UserAction.PlayAgain:
                    StartNewGame();
                    break;
                case GameCompletedPanel.UserAction.Exit:
                    ExitGame();
                    break;
            }
        }

        private void HandleLevelFailed()
        {
            StartCoroutine(UpdateGameOverDelay());
        }

        private void HandleLevelCompleted()
        {
            StartCoroutine(UpdateLevelCompletedDelay());
        }

        private IEnumerator UpdateGameOverDelay()
        {
            yield return new WaitForSeconds(_gameOverPanelDelay);

            ShowGameOverPanel();
        }

        private IEnumerator UpdateLevelCompletedDelay()
        {
            yield return new WaitForSeconds(_nextLevelPanelDelay);

            ShowLevelCompletedPanel();   
        }

        private void ShowLevelCompletedPanel()
        {
            StopAllCoroutines();

            _uiViewsController.ShowUIView(UIViewType.LevelCompletedPanel);
        }

        private void ShowGameOverPanel()
        {
            StopAllCoroutines();

            _uiViewsController.ShowUIView(UIViewType.GameOverPanel);
        }
    
        private void GoToNextLevel()
        {
            _levelNumber++;

            if (_levelNumber <= _sceneLoader.MaxLevelNumber)
            {
                _uiViewsController.ShowUIView(UIViewType.LevelLoadProgress);
                _sceneLoader.LoadLevel(_levelNumber, OnLevelLoadProgress, OnLevelLoadCompleted);
            }
            else
            {
                _uiViewsController.ShowUIView(UIViewType.GameCompletedPanel);
            }
        }

        private void RestartLevel()
        {
            _uiViewsController.ShowUIView(UIViewType.LevelLoadProgress);
            _sceneLoader.LoadLevel(_levelNumber, OnLevelLoadProgress, OnLevelLoadCompleted);
        }

        private void StartNewGame()
        {
            _levelNumber = 1;
            _uiViewsController.ShowUIView(UIViewType.LevelLoadProgress);
            _sceneLoader.LoadLevel(_levelNumber, OnLevelLoadProgress, OnLevelLoadCompleted);
        }


        private void OnLevelLoadProgress(float progressValue)
        {
            _uiViewsController.SetLevelLoadProgress(progressValue);
        }
    
        private void OnLevelLoadCompleted()
        {
            _uiViewsController.HideUIView(UIViewType.LevelLoadProgress);
        
            _levelController.PrepareLevel();

            if (_levelNumber == 1)
            {
                _uiViewsController.ShowUIView(UIViewType.ControlsInfoPanel);
            }
        }
    
        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}