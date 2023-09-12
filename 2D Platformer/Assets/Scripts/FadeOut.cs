using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private bool fadingOut;
    private bool fadingIn;

    private float alphaAdd;
    [SerializeField] private float alphaAddSpeed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(fadingOut)
        {
            spriteRenderer.color = new(0, 0, 0, alphaAdd);
            alphaAdd += alphaAddSpeed * Time.deltaTime;
            if(alphaAdd >= 1)
            {
                fadingOut = false;
            }
        }
        else if(fadingIn)
        {
            spriteRenderer.color = new(0, 0, 0, alphaAdd);
            alphaAdd -= alphaAddSpeed * Time.deltaTime;
            if (alphaAdd <= 0)
            {
                fadingIn = false;
            }
        }
    }

    public void FadeOutScreen()
    {
        alphaAdd = 0;
        fadingOut = true;
    }

    public void FadeInScreen()
    {
        alphaAdd = 1;
        fadingIn = true;
    }
}
