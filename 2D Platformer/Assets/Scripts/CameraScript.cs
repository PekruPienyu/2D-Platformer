using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private FadeOut fadeoutImage;
    private Vector3 startPos;
    private Vector3 playerPos;
    private bool followPlayer;

    public static CameraScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!followPlayer) return;
        playerPos = Player.instance.transform.position;
        if (playerPos.x > transform.position.x) transform.position = new Vector3(playerPos.x, transform.position.y, -10);
    }

    public void SetToStartPosition()
    {
        transform.position = startPos;
        followPlayer = true;
    }

    public void SetNewStartPos(Vector3 newPos)
    {
        startPos = newPos;
    }

    public void SetPosition(Vector3 origin, bool _followPlayer)
    {
        transform.position = origin;
        followPlayer = _followPlayer;
    }

    public void CameraFadeOut()
    {
        fadeoutImage.FadeOutScreen();
    }

    public void CameraFadeIn()
    {
        fadeoutImage.FadeInScreen();
    }

    public void NewSceneConfigure(Vector3 newPos)
    {
        SetNewStartPos(newPos);
        SetToStartPosition();
        CameraFadeIn();
    }
}
