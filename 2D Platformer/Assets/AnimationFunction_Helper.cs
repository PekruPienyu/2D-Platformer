using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunction_Helper : MonoBehaviour
{
    public void PauseGame_Animation()
    {
        MainManager.instance.PauseGame();
    }

    public void ResumeGame_Animation()
    {
        MainManager.instance.ResumeGame();
    }

    public void StopPlayerMovement_Animation()
    {
        gameObject.GetComponent<Player_Controller>().StopPlayerMovement();
    }
}
