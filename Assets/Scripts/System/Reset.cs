using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get all game objects in DontDestroyOnLoad scene
        GameObject[] allDontDestroyOnLoadObjects = SystemRoot.instance.gameObject.scene.GetRootGameObjects();

        // Iterate game objects in DontDestroyOnLoad scene
        foreach (var gameObjectToDestroy in allDontDestroyOnLoadObjects)
        {
            Destroy(gameObjectToDestroy);
        }

        StartCoroutine(LoadMainScene());
    }

    IEnumerator LoadMainScene()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(0);
        while (!loadScene.isDone)
        {
            yield return null;
        }
    }
}
