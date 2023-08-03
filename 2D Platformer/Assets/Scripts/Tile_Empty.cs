using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tile_Empty : Tile_Base
{
    [SerializeField] private GameObject debrisPrefab;
    public override void OnHit()
    {
        if(Player.instance.GetCurrentPower() == 1)
        {
            Bounce();
        }
        else DeactivateAndSpawnDebris();
    }

    public override void SpawnItem()
    {
        return;
    }

    private void DeactivateAndSpawnDebris()
    {
        GameObject debris =  Instantiate(debrisPrefab);
        debris.GetComponent<Debris>().ConfigureDebris(transform.position ,new Vector2(1, 4));
        debris = Instantiate(debrisPrefab);
        debris.GetComponent<Debris>().ConfigureDebris(transform.position,new Vector2(-1, 4));
        debris = Instantiate(debrisPrefab);
        debris.GetComponent<Debris>().ConfigureDebris(transform.position,new Vector2(1, 2));
        debris = Instantiate(debrisPrefab);
        debris.GetComponent<Debris>().ConfigureDebris(transform.position,new Vector2(-1, 2));

        gameObject.SetActive(false);
    }
}
