using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MockMenu : MonoBehaviour
{
    [SerializeField] private GameEvent _levelLoadedEvent;
    
    public void StartGame()
    {
        _levelLoadedEvent.AddListener(OnLevelLoaded);
        MatchSystem.instance.StartGame(1);
    }

    private void OnLevelLoaded()
    {
        _levelLoadedEvent.RemoveListener(OnLevelLoaded);
        StartCoroutine(UnloadScene(2));
    }
    
    private IEnumerator UnloadScene(int sceneIndex)
    {
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(sceneIndex, UnloadSceneOptions.None);
        while (!unloadScene.isDone)
        {
            yield return null;
        }
    }
}
