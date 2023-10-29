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
        Debug.Log(currentlyLoadedEditorScene.name);
        if(currentlyLoadedEditorScene.name == "BootstrapScene")
        {
            Debug.Log("Bruh");
            return;
        }
        if (SceneManager.GetSceneByName("LevelTestBootstrapScene").isLoaded != true)
        {
            Debug.Log("WHY");
            SceneManager.LoadScene("LevelTestBootstrapScene");
        }

        if (currentlyLoadedEditorScene.IsValid())
        {
            SceneManager.LoadSceneAsync(currentlyLoadedEditorScene.name, LoadSceneMode.Additive);
        }
    }
    #endif
}
