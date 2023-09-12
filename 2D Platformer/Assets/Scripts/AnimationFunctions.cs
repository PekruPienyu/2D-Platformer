using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{
    [SerializeField] private BulletScript bullet;

    public void Deactivate_Animation()
    {
        transform.root.gameObject.SetActive(false);
    }

    public void DisableBoxCollider_Animation()
    {
        bullet.DisableBoxCollider();
        bullet.Explode();
    }
}
