using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRoomManager : MonoBehaviour
{
    public static SecretRoomManager instance;

    private GameObject entranceSpawnPointCam;
    private GameObject exitSpawnPointCam;
    private GameObject entranceSpawnPointPlayer;
    private GameObject exitSpawnPointPlayer;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ConfigureSecretRoom(SecretRoom_Helper helper)
    {
        entranceSpawnPointCam = helper.entranceSpawnPointCam;
        exitSpawnPointCam = helper.exitSpawnPointCam;
        entranceSpawnPointPlayer = helper.entranceSpawnPointPlayer;
        exitSpawnPointPlayer = helper.exitSpawnPointPlayer;
    }

    public void EnterSecretRoom()
    {
        CameraScript.instance.CameraFadeOut();
        StartCoroutine(PlayerEnterSecretRoomConfigure());
    }

    private IEnumerator PlayerEnterSecretRoomConfigure()
    {
        yield return new WaitForSeconds(0.5f);

        CameraScript.instance.SetPosition(entranceSpawnPointCam.transform.position, false);
        Player.instance.SetPosition(entranceSpawnPointPlayer.transform.position);
        Player.instance.EnterSecretRoomConfigure();
        CameraScript.instance.CameraFadeIn();
    }

    public void ExitSecretRoom()
    {
        CameraScript.instance.CameraFadeOut();
        StartCoroutine(PlayerExitSecretRoomConfigure());
    }

    private IEnumerator PlayerExitSecretRoomConfigure()
    {
        yield return new WaitForSeconds(0.5f);

        CameraScript.instance.SetPosition(exitSpawnPointCam.transform.position, true);
        Player.instance.SetPosition(exitSpawnPointPlayer.transform.position);
        Player.instance.ExitSecretRoomConfigure();
        CameraScript.instance.CameraFadeIn();
    }
}
