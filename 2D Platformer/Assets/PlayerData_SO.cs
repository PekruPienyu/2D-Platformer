using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerData")]
public class PlayerData_SO : ScriptableObject
{
    public Vector3 spawnPos;
    public int score;
    public int coin;
    public int live;
    public int worldIndex;
    public int powerLevel;
}
