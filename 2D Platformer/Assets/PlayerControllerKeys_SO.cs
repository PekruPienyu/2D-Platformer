using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerController")]
public class PlayerControllerKeys_SO : ScriptableObject
{
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode sprint;
}
