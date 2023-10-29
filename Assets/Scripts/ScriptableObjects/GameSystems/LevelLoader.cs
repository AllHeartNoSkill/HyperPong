using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : Singleton<LevelLoader>
{
    [SerializeField] private int _sceneOffset = 3;
    [Header("Game Events")] 
    [SerializeField] private GameEvent _levelLoadedEvent;
    [SerializeField] private GameEvent _playerLoadedEvent;
    [SerializeField] private GameEvent _levelUnloadedEvent;
    [SerializeField] private GameEvent _matchReadyEvent;

    public void LoadLevel(int level, bool loadPlayer = true)
    {
        StartCoroutine(LoadLevelScene(level));
    }

    public void LoadPlayer()
    {
        StartCoroutine(LoadPlayerScene());
    }

    public void UnloadLevel(int level)
    {
        StartCoroutine(UnloadLevelScene(level));
    }

    public void UnloadPlayer()
    {
        StartCoroutine(UnloadPlayerScene());
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadSceneAsync(3, LoadSceneMode.Additive));
    }

    public void OnlyLoadPlayer()
    {
        StartCoroutine(LoadPlayerOnly());
        IEnumerator LoadPlayerOnly()
        {
            
            AsyncOperation loadScene = SceneManager.LoadSceneAsync(PlayerSceneIndex, mode: LoadSceneMode.Additive);
            while (!loadScene.isDone)
            {
                yield return null;
            }
        
            _levelLoadedEvent.TriggerEvent();
            _playerLoadedEvent.TriggerEvent();
            // _matchReadyEvent.TriggerEvent();
        }
    }

    private IEnumerator UnloadLevelScene(int level)
    {
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(LevelSceneIndex(level), UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        while (!unloadScene.isDone)
        {
            yield return null;
        }
        
        _levelUnloadedEvent.TriggerEvent();
    }

    private IEnumerator UnloadPlayerScene()
    {
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(PlayerSceneIndex, UnloadSceneOptions.None);
        while (!unloadScene.isDone)
        {
            yield return null;
        }

    }
    
    private IEnumerator LoadLevelScene(int level)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(LevelSceneIndex(level), mode: LoadSceneMode.Additive);
        while (!loadScene.isDone)
        {
            yield return null;
        }
        
        _levelLoadedEvent.TriggerEvent();
        // _matchReadyEvent.TriggerEvent();
    }

    private IEnumerator LoadPlayerScene()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(PlayerSceneIndex, mode: LoadSceneMode.Additive);
        while (!loadScene.isDone)
        {
            yield return null;
        }
        
        _playerLoadedEvent.TriggerEvent();
    }

    private IEnumerator LoadSceneAsync(int sceneIndex, LoadSceneMode mode)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneIndex, mode);
        while (!loadScene.isDone)
        {
            yield return null;
        }
    }

    private int LevelSceneIndex(int level) => level + _sceneOffset;
    private int PlayerSceneIndex => _sceneOffset;
}
