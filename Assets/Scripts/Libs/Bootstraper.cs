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
        return;
        
        var currentlyLoadedEditorScene = SceneManager.GetActiveScene();
        
        if(currentlyLoadedEditorScene.name == "BootstrapScene")
        {
            return;
        }

        if (currentlyLoadedEditorScene.name.Contains("LevelScene"))
        {
            if (SceneManager.GetSceneByName("LevelTestBootstrapScene").isLoaded != true)
            {
                SceneManager.LoadScene("LevelTestBootstrapScene");
            }
        }
        else if (currentlyLoadedEditorScene.name.Contains("Menu"))
        {
            if (SceneManager.GetSceneByName("MenuTestBootstrapScene").isLoaded != true)
            {
                SceneManager.LoadScene("MenuTestBootstrapScene");
            }
        }
        else
        {
            return;
        }

        if (currentlyLoadedEditorScene.IsValid())
        {
            SceneManager.LoadSceneAsync(currentlyLoadedEditorScene.name, LoadSceneMode.Additive);
        }
    }
    #endif
}
