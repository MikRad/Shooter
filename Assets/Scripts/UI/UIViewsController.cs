using System.Collections.Generic;
using DI;
using DI.Services;
using Factories.Config.UI;
using Factories.UI;
using UnityEngine;

namespace UI
{
    public class UIViewsController : MonoBehaviour
    {
        [Header("Container")]
        [SerializeField] private Transform _canvasTransform;
    
        private readonly Dictionary<UIViewType, UIView> _uiViewsMap = new Dictionary<UIViewType, UIView>();
        private UIViewFactory _uiViewFactory;

        public void Init(DIContainer diContainer)
        {
            _uiViewFactory = diContainer.Resolve<UIViewFactory>();
        
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

        public void SetLevelLoadProgress(float progressValue)
        {
            LevelLoadProgressPanel levelLoadPanel = _uiViewsMap[UIViewType.LevelLoadProgress] as LevelLoadProgressPanel; 
            levelLoadPanel?.SetProgressValue(progressValue);
        }
    
        private void CreateUIViewsMap()
        {
            UIViewFactoryConfig.UIViewCreateInfo[] infos = _uiViewFactory.GetUIViewCreateInfos();

            foreach (UIViewFactoryConfig.UIViewCreateInfo info in infos)
            {
                _uiViewsMap.TryAdd(info.type, Instantiate(info.viewPrefab, _canvasTransform));
            }
        }
    }
}
