using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private GameObject camStartPoint;
    [SerializeField] private GameObject playerStartPoint;
    [HideInInspector] public Vector3 playerSavePoint;

    private void Start()
    {
        playerSavePoint = playerStartPoint.transform.position;
        MainManager.instance.ConfigureNewSceneLoad(this);
    }

    public void SceneLoadConfigure(bool isNewScene)
    {
        if(isNewScene)
        {
            Player.instance.ConfigureNewSceneLoad(playerSavePoint);
        }
        CameraScript.instance.NewSceneConfigure(camStartPoint.transform.position);
        Player.instance.ResetPlayerPosition();
    }

    public void SetPlayerSavePoint(Vector3 savePoint)
    {
        playerSavePoint = savePoint;
    }

    public void ResetPlayerSavePoint()
    {
        playerSavePoint = playerStartPoint.transform.position;
    }
}
