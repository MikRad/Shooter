using System.Collections.Generic;
using UnityEngine;

public class UIViewsController : MonoBehaviour
{
    [Header("Container")]
    [SerializeField] private Transform _canvasTransform;
    
    [Header("Prefabs")] 
    [SerializeField] private PlayerUIStats _playerUIStatsPrefab;
    [SerializeField] private BossUIStats _bossUIStatsPrefab;
    [SerializeField] private LevelCompletedPanel _levelCompletedPanelPrefab;
    [SerializeField] private GameOverPanel _gameOverPanelPrefab;
    [SerializeField] private GameCompletedPanel _gameCompletedPanelPrefab;
    
    private readonly Dictionary<UIViewType, UIView> _uiViewsMap = new Dictionary<UIViewType, UIView>();

    protected void Awake()
    {
        CreateUIViewsMap();
    }

    public void ShowUIView(UIViewType viewType)
    {
        _uiViewsMap[viewType].Show();
    }

    public void HideUIView(UIViewType viewType)
    {
        _uiViewsMap[viewType].Hide();
    }

    public void ResetPlayerUIStats()
    {
        PlayerUIStats playerUIStats = _uiViewsMap[UIViewType.PlayerUIStats] as PlayerUIStats;
        playerUIStats?.Reset();
    }
    
    public void ResetBossUIStats()
    {
        BossUIStats bossUIStats = _uiViewsMap[UIViewType.BossUIStats] as BossUIStats;
        bossUIStats?.Reset();
    }
    
    private void CreateUIViewsMap()
    {
        _uiViewsMap.Add(UIViewType.PlayerUIStats, Instantiate(_playerUIStatsPrefab, _canvasTransform));
        _uiViewsMap.Add(UIViewType.BossUIStats, Instantiate(_bossUIStatsPrefab, _canvasTransform));
        _uiViewsMap.Add(UIViewType.LevelCompletedPanel, Instantiate(_levelCompletedPanelPrefab, _canvasTransform));
        _uiViewsMap.Add(UIViewType.GameOverPanel, Instantiate(_gameOverPanelPrefab, _canvasTransform));
        _uiViewsMap.Add(UIViewType.GameCompletedPanel, Instantiate(_gameCompletedPanelPrefab, _canvasTransform));
    }
}
