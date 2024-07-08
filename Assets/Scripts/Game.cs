using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private DIContainer _diContainer;

    private void Awake()
    {
        FindServices();
        RegisterServices();
        AddServicesEventHandlers();
    }

    private void Start()
    {
        _levelController.Init(_diContainer);
    }

    private void FindServices()
    {
        _uiViewsController = GetComponentInChildren<UIViewsController>();
        _levelController = GetComponentInChildren<LevelController>();
        _audioController = GetComponentInChildren<AudioController>();
        _bulletSpawner = GetComponentInChildren<BulletSpawner>();
        _pickupItemSpawner = GetComponentInChildren<PickupItemSpawner>();
        _vfxSpawner = GetComponentInChildren<VfxSpawner>();
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
        _diContainer.RegisterSingleton<IPlayerInput>((c) => new DesktopPlayerInput());
        _diContainer.RegisterSingleton<PlayerFactory>((c) => new PlayerFactory(c));
    }
    
    private void OnDestroy()
    {
        RemoveServicesEventHandlers();
    }

    private void AddServicesEventHandlers()
    {
        EventBus.Get.Subscribe<LevelCompletedPanelClosedEvent>(HandleLevelCompletedPanelClosed);
        EventBus.Get.Subscribe<GameOverPanelClosedEvent>(HandleGameOverPanelClosed);
        EventBus.Get.Subscribe<GameCompletedPanelClosedEvent>(HandleGameCompletedPanelClosed);
        
        EventBus.Get.Subscribe<LevelCompletedEvent>(HandleLevelCompleted);
        EventBus.Get.Subscribe<LevelFailedEvent>(HandleLevelFailed);
    }
    
    private void RemoveServicesEventHandlers()
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
    
    private void ShowGameCompletedPanel()
    {
        _uiViewsController.ShowUIView(UIViewType.GameCompletedPanel);
    }

    private void GoToNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            ShowGameCompletedPanel();
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void StartNewGame()
    {
        SceneManager.LoadScene(0);
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