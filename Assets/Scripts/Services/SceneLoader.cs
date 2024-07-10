using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string ConfigPath = "Configs/SceneLoaderConfig";
    private SceneLoaderConfig _config;
    private AsyncOperation _asyncOperation;

    private void Awake()
    {
        LoadConfig();
    }

    public void LoadLevel(int levelNumber, Action<float> onProgress, Action onComplete, float minLoadTime)
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
        
        StartCoroutine(LoadSceneCoroutine(sceneName, onProgress, onComplete, minLoadTime));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, Action<float> onProgress, Action onComplete, float minLoadTime)
    {
        float startTime = Time.time;

        _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        _asyncOperation.allowSceneActivation = false;

        if (_asyncOperation == null)
        {
            Debug.LogError($"Failed to load scene {sceneName} !");
            yield break;
        }

        while (!_asyncOperation.isDone)
        {
            if (_asyncOperation.progress >= 0.9f && Time.time - startTime >= minLoadTime)
            {
                _asyncOperation.allowSceneActivation = true;
            }

            if (_asyncOperation.allowSceneActivation && _asyncOperation.isDone)
            {
                onComplete?.Invoke();
                _asyncOperation = null;
                yield break;
            }

            onProgress?.Invoke(_asyncOperation.progress / 0.9f);
            yield return null;
        }
    }
    
    private void LoadConfig()
    {
        _config = Resources.Load<SceneLoaderConfig>(ConfigPath);        
    }
}