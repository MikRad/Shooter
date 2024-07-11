using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLoaderConfig", menuName = "ScriptableObjects/SceneLoaderConfig", order = 1)]
public class SceneLoaderConfig : ScriptableObject
{
    [SerializeField] private LevelLoadInfo[] _levelLoadInfos;

    private readonly Dictionary<int, string> _levelLoadInfosMap = new Dictionary<int, string>();

    public int LevelsNumberTotal => _levelLoadInfos.Length;
    
    private void OnEnable()
    {
        FillLevelLoadInfosMap();
    }

    public string GetLevelSceneName(int levelNumber)
    {
        return _levelLoadInfosMap.TryGetValue(levelNumber, out string sceneName) ? sceneName : null;
    }
    
    private void FillLevelLoadInfosMap()
    {
        _levelLoadInfosMap.Clear();

        if (_levelLoadInfos == null)
            return;

        foreach (LevelLoadInfo info in _levelLoadInfos)
        {
            _levelLoadInfosMap.TryAdd(info.levelNumber, info.sceneName);
        }
    }

    [Serializable]
    private struct LevelLoadInfo
    {
        public int levelNumber;
        public string sceneName;
    }
}
