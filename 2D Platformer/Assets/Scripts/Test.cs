using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEnemyScript;

public class Test : MonoBehaviour
{
    public GameObject bulletPrefab;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CreateBullet();
        }
    }

    private void CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
    }
}
