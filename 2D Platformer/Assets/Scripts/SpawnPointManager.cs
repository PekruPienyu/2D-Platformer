using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private GameObject camStartPoint;
    [SerializeField] private GameObject playerStartPoint;

    [SerializeField] private GameObject secretEntranceSpawnPointCam;
    [SerializeField] private GameObject secretExitSpawnPointCam;
    [SerializeField] private GameObject secretEntranceSpawnPointPlayer;
    [SerializeField] private GameObject secretExitSpawnPointPlayer;
    private void Start()
    {
        if(SceneLoader.instance.isNewScene)
        {
            NewSceneLoadConfigure();
        }
        else
        {
            SceneReloadConfigure();
        }
    }

    public void NewSceneLoadConfigure()
    {
        Player.instance.SetNewSpawnPos(playerStartPoint.transform.position);
        CameraScript.instance.SetNewStartPos(camStartPoint);
        Player.instance.ResetPlayerPosition();
        Player.instance.ConfigureNewSceneLoad();
        CameraScript.instance.SetToStartPosition();
        CameraScript.instance.CameraFadeIn();
    }

    public void SceneReloadConfigure()
    {
        CameraScript.instance.SetNewStartPos(camStartPoint);
        Player.instance.LoadPlayerData_NewScene();
        Player.instance.ResetPlayerPosition();
        CameraScript.instance.SetToStartPosition();
        CameraScript.instance.CameraFadeIn();
    }

    public void EnterSecretRoom()
    {
        CameraScript.instance.SetPosition(secretEntranceSpawnPointCam.transform.position, false);
        Player.instance.SetPosition(secretEntranceSpawnPointPlayer.transform.position);
    }

    public void ExitSecretRoom()
    {
        CameraScript.instance.SetPosition(secretExitSpawnPointCam.transform.position, true);
        Player.instance.SetPosition(secretExitSpawnPointPlayer.transform.position);
    }

}
