using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : Singleton<LevelLoader>
{
    [Header("Game Events")] 
    [SerializeField] private GameEvent _levelLoadedEvent;
    [SerializeField] private GameEvent _playerLoadedEvent;
    [SerializeField] private GameEvent _levelUnloadedEvent;
    [SerializeField] private GameEvent _matchReadyEvent;

    public void LoadLevel(int level, bool loadPlayer = true)
    {
        if (loadPlayer)
        {
            StartCoroutine(LoadPlayerScene(level));
        }
        else
        {
            StartCoroutine(LoadLevelScene(level));
        }
    }

    public void UnloadLevel(int level)
    {
        StartCoroutine(UnloadLevelScene(level));
    }

    private IEnumerator UnloadLevelScene(int level)
    {
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(LevelSceneIndex(level), UnloadSceneOptions.None);
        while (!unloadScene.isDone)
        {
            yield return null;
        }
        
        _levelUnloadedEvent.TriggerEvent();
    }
    
    private IEnumerator LoadLevelScene(int level)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(LevelSceneIndex(level), mode: LoadSceneMode.Additive);
        while (!loadScene.isDone)
        {
            yield return null;
        }
        
        _levelLoadedEvent.TriggerEvent();
        _matchReadyEvent.TriggerEvent();
    }

    private IEnumerator LoadPlayerScene(int level)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(PlayerSceneIndex, mode: LoadSceneMode.Additive);
        while (!loadScene.isDone)
        {
            yield return null;
        }
        
        _playerLoadedEvent.TriggerEvent();
        StartCoroutine(LoadLevelScene(level));
    }

    private int LevelSceneIndex(int level) => level + 1;
    private int PlayerSceneIndex => 1;
}
