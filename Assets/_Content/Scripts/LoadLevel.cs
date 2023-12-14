using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public event Action RequestedLoadLevelEvent;

    private int _requestedLevel;
    private bool _isWaitForLoad = false;

    public void Load(int level)
    {
        if (level > 0 && level < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(level);
        }
    }

    public void LoadNext()
    {
        if (IsLastLevel()) return;
        Load(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RetryLevel()
    {
        Load(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void RequestLevel(int level)
    {
        if (_isWaitForLoad) return;
        
        if (level > 0 && level < SceneManager.sceneCountInBuildSettings)
        {
            _requestedLevel = level;
            _isWaitForLoad = true;
            RequestedLoadLevelEvent?.Invoke();
        }
    }

    public void RequestLoadNext()
    {
        if (_isWaitForLoad) return;
        if (IsLastLevel()) return;
        
        _requestedLevel = SceneManager.GetActiveScene().buildIndex + 1;
        _isWaitForLoad = true;
        RequestedLoadLevelEvent?.Invoke();
    }
    
    public void LoadRequested()
    {
        if (!_isWaitForLoad) return;
        
        SceneManager.LoadScene(_requestedLevel);
        _isWaitForLoad = false;
    }

    public bool IsLastLevel()
    {
        return SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings;
    }

    // private IEnumerator LoadSceneCoroutine(int index)
    // {
    //     SceneStartLoadingEvent?.Invoke();
    //
    //     AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
    //     while (!asyncLoad.isDone) yield return null;
    //
    //     SceneLoadedEvent?.Invoke();
    // }
}
