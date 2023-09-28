using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerData")]
public class PlayerData_SO : ScriptableObject
{
    public Vector3 spawnPos;
    public int score = 0;
    public int coin = 0;
    public int live = 3;
    public int worldIndex = 1;
    public int powerLevel = 1;

    public void ResetData()
    {
        score = 0;
        coin = 0;
        live = 3;
        worldIndex = 1;
        powerLevel = 1;
    }
}
