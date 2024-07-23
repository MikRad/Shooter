using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string ConfigPath = "Configs/SceneLoaderConfig";
    private SceneLoaderConfig _config;
    private AsyncOperation _asyncOperation;

    public int MaxLevelNumber => _config.LevelsNumberTotal;
    
    private void Awake()
    {
        LoadConfig();
    }

    public void LoadLevel(int levelNumber, Action<float> onProgress, Action onCompleted)
    {
        string sceneName = _config.GetLevelSceneName(levelNumber);

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"Scene name for level number {levelNumber} is null or empty !");
            return;
        }        
        if (_asyncOperation != null)
        {
            Debug.LogWarning($"Trying to load scene {sceneName} while scene load in progress !");
            return;
        }
        
        StartCoroutine(LoadSceneCoroutine(sceneName, onProgress, onCompleted));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, Action<float> onProgress, Action onCompleted)
    {
        _asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        if (_asyncOperation == null)
        {
            Debug.LogError($"Failed to load scene {sceneName} !");
            yield break;
        }
        
        _asyncOperation.allowSceneActivation = false;
        float loadProgress = 0f;
        
        while (!_asyncOperation.isDone)
        {
            loadProgress = Mathf.MoveTowards(loadProgress, _asyncOperation.progress, Time.deltaTime);
            
            if (loadProgress >= 0.9f)
            {
                loadProgress = 1f;
                _asyncOperation.allowSceneActivation = true;
            }

            onProgress?.Invoke(loadProgress);
            yield return null;
        }
        
        onCompleted?.Invoke();
        _asyncOperation = null;
    }
    
    private void LoadConfig()
    {
        _config = Resources.Load<SceneLoaderConfig>(ConfigPath);        
    }
}