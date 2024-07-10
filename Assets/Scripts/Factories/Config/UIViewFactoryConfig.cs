using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UIViewFactoryConfig", menuName = "ScriptableObjects/UIViewFactoryConfig", order = 1)]
public class UIViewFactoryConfig : ScriptableObject
{
    [SerializeField] private UIViewCreateInfo[] _createInfos;

    public UIViewCreateInfo[] UIViewCreateInfos => _createInfos;
    
    [Serializable]
    public struct UIViewCreateInfo
    {
        public UIViewType type;
        public UIView viewPrefab;
    }
}