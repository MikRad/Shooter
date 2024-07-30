using System.Collections.Generic;
using Factories.Config.UI;
using UI;
using UnityEngine;

namespace Factories.UI
{
    public class UIViewFactory
    {
        private const string ConfigPath = "Configs/UIViewFactoryConfig";
        private readonly UIViewFactoryConfig _config;
        private readonly Dictionary<UIViewType, UIView> _uiViewPrefabsMap = new Dictionary<UIViewType, UIView>();
    
        public UIViewFactory(IResourcesDataProvider dataProvider)
        {
            _config = dataProvider.LoadResource<UIViewFactoryConfig>(ConfigPath);
            
            FillPrefabsMap();
        }

        public IEnumerable<UIViewType> GetUIViewTypes()
        {
            return _uiViewPrefabsMap.Keys;
        }
    
        public UIView Create(UIViewType viewType, Transform canvasTransform)
        {
            UIView view = null;
            if (_uiViewPrefabsMap.TryGetValue(viewType, out UIView prefab))
            {
                view = Object.Instantiate(prefab, canvasTransform);
            }

            return view;
        }
        
        private void FillPrefabsMap()
        {
            foreach (UIViewFactoryConfig.UIViewCreateInfo info in _config.UIViewCreateInfos)
            {
                _uiViewPrefabsMap.TryAdd(info.type, info.viewPrefab);
            }
        }
    }
}