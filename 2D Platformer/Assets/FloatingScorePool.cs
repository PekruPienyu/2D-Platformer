using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FloatingScorePool : MonoBehaviour
{
    [HideInInspector] public static FloatingScorePool instance;
    [SerializeField] private GameObject floatingScorePrefab;

    public IObjectPool<GameObject> pool;

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
    }

    private void Start()
    {
        pool = new ObjectPool<GameObject>(CreatePoolItem, OnGetFromPool, OnReturnToPool);
    }

    private GameObject CreatePoolItem()
    {
        return Instantiate(floatingScorePrefab);
    }

    private void OnGetFromPool(GameObject item)
    {
        item.SetActive(true);
    }

    private void OnReturnToPool(GameObject item)
    {
        item.SetActive(false);
    }

    public void GetFromPool(Vector3 origin, int score)
    {
        GameObject item =  pool.Get();
        item.GetComponent<FloatingScore>().Configure(origin, score);
    }
}
