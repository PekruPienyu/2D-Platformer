using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRoomManager : MonoBehaviour
{
    public static SecretRoomManager instance;

    [SerializeField] private GameObject entranceSpawnPointCam;
    [SerializeField] private GameObject exitSpawnPointCam;
    [SerializeField] private GameObject entranceSpawnPointPlayer;
    [SerializeField] private GameObject exitSpawnPointPlayer;

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

    public void EnterSecretRoom()
    {
        CameraScript.instance.SetPosition(entranceSpawnPointCam.transform.position, false);
        Player.instance.SetPosition(entranceSpawnPointPlayer.transform.position);
    }

    public void ExitSecretRoom()
    {
        CameraScript.instance.SetPosition(exitSpawnPointCam.transform.position, true);
        Player.instance.SetPosition(exitSpawnPointPlayer.transform.position);
    }
}
