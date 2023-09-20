using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private Transform camStartPoint;
    [SerializeField] private Transform playerStartPoint;

    private Vector3 playerSavePoint;

    public static SpawnPointManager instance;

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
        DontDestroyOnLoad(camStartPoint.gameObject);
        DontDestroyOnLoad(playerStartPoint.gameObject);
        playerSavePoint = playerStartPoint.position;
    }

    public void SceneLoadConfigure()
    {
        if(playerSavePoint != playerStartPoint.position)
        {
            MainManager.instance.ConfigureSavePointLoad();
        }
        else
        {
            MainManager.instance.ConfigureNewSceneLoad();
        }
        Player.instance.SetNewSpawnPos(playerSavePoint);
        CameraScript.instance.NewSceneConfigure(camStartPoint.position);
        Player.instance.ResetPlayerPosition();
    }

    public void SetPlayerSavePoint(Vector3 savePoint)
    {
        playerSavePoint = savePoint;
    }

    public void ResetPlayerSavePoint()
    {
        playerSavePoint = playerStartPoint.position;
    }
}
