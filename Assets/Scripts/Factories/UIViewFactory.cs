using UnityEngine;

public class UIViewFactory
{
    private const string ConfigPath = "Configs/UIViewFactoryConfig";
    private UIViewFactoryConfig _config;
    
    public UIViewFactory()
    {
        LoadConfig();
    }

    public UIViewFactoryConfig.UIViewCreateInfo[] GetUIViewCreateInfos()
    {
        return _config.UIViewCreateInfos;
    }
    
    private void LoadConfig()
    {
        _config = Resources.Load<UIViewFactoryConfig>(ConfigPath);        
    }
}