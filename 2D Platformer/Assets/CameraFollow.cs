using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private Vector3 playerPos;

    private void Update()
    {
        playerPos = player.transform.position;
        if(playerPos.x > transform.position.x)transform.position = new Vector3(playerPos.x, transform.position.y, -10);
    }
}
