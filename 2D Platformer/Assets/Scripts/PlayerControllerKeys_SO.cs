using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerController")]
public class PlayerControllerKeys_SO : ScriptableObject
{
    public KeyCode left;
    public KeyCode right;
    public KeyCode down;
    public KeyCode jump;
    public KeyCode sprint;

    public PlayerControllerKeys_SO()
    {
        ResetToDefaultKeys();
    }

    public void ResetToDefaultKeys()
    {
        left = KeyCode.A;
        right = KeyCode.D;
        down = KeyCode.S;
        jump = KeyCode.Space;
        sprint = KeyCode.LeftShift;
    }
}
