using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentReference_Helper : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject parent;

    public void OnHit(bool popOut)
    {
        parent.GetComponent<IDamageable>().OnHit(popOut);
    }
}
