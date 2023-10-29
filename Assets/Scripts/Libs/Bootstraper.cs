using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstraper : MonoBehaviour
{
    #if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        var currentlyLoadedEditorScene = SceneManager.GetActiveScene();
        
        if(currentlyLoadedEditorScene.name == "BootstrapScene")
        {
            return;
        }

        if (!currentlyLoadedEditorScene.name.Contains("LevelScene"))
        {
            return;
        }
        
        if (SceneManager.GetSceneByName("LevelTestBootstrapScene").isLoaded != true)
        {
            SceneManager.LoadScene("LevelTestBootstrapScene");
        }

        if (currentlyLoadedEditorScene.IsValid())
        {
            SceneManager.LoadSceneAsync(currentlyLoadedEditorScene.name, LoadSceneMode.Additive);
        }
    }
    #endif
}
