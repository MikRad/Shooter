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
    private DiContainer _diContainer;

    private void Start()
    {
        FindServices();
        RegisterServices();
        AddServicesEventHandlers();

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
        _diContainer = new DiContainer();
        _diContainer.Register(_uiViewsController);
        _diContainer.Register(_levelController);
        _diContainer.Register(_audioController);
        _diContainer.Register(_bulletSpawner);
        _diContainer.Register(_pickupItemSpawner);
        _diContainer.Register(_vfxSpawner);
    }
    
    private void OnDestroy()
    {
        RemoveServicesEventHandlers();
    }

    private void AddServicesEventHandlers()
    {
        _uiViewsController.AddUIEventSubscriber(UIEventType.LevelCompletedContinueClick, GoToNextLevel);
        _uiViewsController.AddUIEventSubscriber(UIEventType.GameOverPlayAgainClick, RestartLevel);
        _uiViewsController.AddUIEventSubscriber(UIEventType.GameOverExitClick, ExitGame);
        _uiViewsController.AddUIEventSubscriber(UIEventType.GameCompletedPlayAgainClick, StartNewGame);
        _uiViewsController.AddUIEventSubscriber(UIEventType.GameCompletedExitClick, ExitGame);

        _levelController.OnLevelCompleted += HandleLevelCompleted;
        _levelController.OnLevelFailed += HandleLevelFailed;
    }
    
    private void RemoveServicesEventHandlers()
    {
        _uiViewsController.RemoveUIEventSubscriber(UIEventType.LevelCompletedContinueClick, GoToNextLevel);
        _uiViewsController.RemoveUIEventSubscriber(UIEventType.GameOverPlayAgainClick, RestartLevel);
        _uiViewsController.RemoveUIEventSubscriber(UIEventType.GameOverExitClick, ExitGame);
        _uiViewsController.RemoveUIEventSubscriber(UIEventType.GameCompletedPlayAgainClick, StartNewGame);
        _uiViewsController.RemoveUIEventSubscriber(UIEventType.GameCompletedExitClick, ExitGame);

        _levelController.OnLevelCompleted -= HandleLevelCompleted;
        _levelController.OnLevelFailed -= HandleLevelFailed;
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

    private void GoToNextLevel(object param)
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

    private void RestartLevel(object param)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void StartNewGame(object param)
    {
        SceneManager.LoadScene(0);
    }
    
    private void ExitGame(object param)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}