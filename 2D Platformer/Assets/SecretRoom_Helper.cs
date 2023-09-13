using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRoom_Helper : MonoBehaviour
{
    public GameObject entranceSpawnPointCam;
    public GameObject exitSpawnPointCam;
    public GameObject entranceSpawnPointPlayer;
    public GameObject exitSpawnPointPlayer;

    public void SecretRoomConfigure()
    {
        SecretRoomManager.instance.ConfigureSecretRoom(this);
    }
}
