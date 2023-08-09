using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    private bool isActive;
    private float timer;
    [SerializeField] private float activeDuration;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(isActive)
        {
            timer += Time.deltaTime;
            if(timer >= activeDuration)
            {
                Destroy(gameObject);
            }
        }
    }

    private bool forceAdded = false;

    private void FixedUpdate()
    {
        if (!forceAdded)
        {
            forceAdded = true;
            rb.AddForce(direction, ForceMode2D.Impulse);
        }
    }

    public void ConfigureDebris(Vector3 origin, Vector2 _direction)
    {
        transform.position = origin;
        direction = _direction;
        isActive = true;
    }
}
