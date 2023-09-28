using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : MonoBehaviour
{
    public Action onLoadCallBack;
    public static SceneLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNextScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        int sceneLoadIndex;
        if (scene.buildIndex == 5)
        {
            sceneLoadIndex = 0;
        }
        else sceneLoadIndex = scene.buildIndex + 1;

        onLoadCallBack = () =>
        {
            if (sceneLoadIndex == 0)
            {
                UIManager.instance.LoadMainMenuPanel();
            }
            StartCoroutine(LoadSceneAsync(sceneLoadIndex));
        };

        LoadScene(5);
    }

    public IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncOperation.isDone)
        {
            yield return null;
            UIManager.instance.UpdateWorldTitle((sceneIndex).ToString());
            SpawnPointManager.instance.SceneLoadConfigure();
        }
    }

    public void LoadCallBack()
    {
        if (onLoadCallBack != null)
        {
            onLoadCallBack();
            onLoadCallBack = null;
        }
    }

    public int GetCurrentSceneIndex()
    {
        Scene scene = SceneManager.GetActiveScene();
        return scene.buildIndex;
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
        SpawnPointManager.instance.SceneLoadConfigure();
    }

    public void LoadSavedScene()
    {
        MainManager.instance.LoadPlayerData_NewScene();
        int sceneLoadIndex = MainManager.instance.GetPlayerData().worldIndex;
        onLoadCallBack = () =>
        {
            StartCoroutine(LoadSceneAsync(sceneLoadIndex));
        };

        LoadScene(5);
    }
}
