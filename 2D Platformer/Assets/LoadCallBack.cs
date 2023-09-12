using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCallBack : MonoBehaviour
{
    private float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 1f)
        {
            SceneLoader.instance.LoadCallBack();
        }
    }
}
