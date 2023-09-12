using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_Functions : MonoBehaviour
{
    public void DeactivateObject_AnimaFunc()
    {
        gameObject.SetActive(false);
        gameObject.transform.root.gameObject.SetActive(false);
    }
}
