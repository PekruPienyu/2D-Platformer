using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingScore : MonoBehaviour
{
    bool isActive = false;
    [SerializeField] private TMP_Text floatingScoreText;

    public void Configure(Vector3 origin, int _score)
    {
        transform.position = origin;
        isActive = true;
        floatingScoreText.text = "" + _score;
    }

    float timer;

    private void Update()
    {
        if(isActive)
        {
            timer += Time.deltaTime;
            if(timer >= 1)
            {
                timer = 0;
                isActive = false;
                FloatingScorePool.instance.pool.Release(gameObject);
            }

            transform.Translate(Vector2.up * Time.deltaTime * 2);
        }
    }
}