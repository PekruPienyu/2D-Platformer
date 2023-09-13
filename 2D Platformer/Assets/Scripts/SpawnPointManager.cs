using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private GameObject camStartPoint;
    [SerializeField] private GameObject playerStartPoint;
    private void Start()
    {
        SceneLoadConfigure();
    }

    public void SceneLoadConfigure()
    {
        if(SceneLoader.instance.isNewScene)
        {
            Player.instance.SetNewSpawnPos(playerStartPoint.transform.position);
            Player.instance.ConfigureNewSceneLoad();
        }
        else
        {
            Player.instance.LoadPlayerData_NewScene();
        }
        CameraScript.instance.SetNewStartPos(camStartPoint);
        Player.instance.ResetPlayerPosition();
        CameraScript.instance.SetToStartPosition();
        CameraScript.instance.CameraFadeIn();
    }
}
