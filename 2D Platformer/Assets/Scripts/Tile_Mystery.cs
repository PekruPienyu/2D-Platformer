using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Mystery : Tile_Base
{

    [SerializeField] private List<GameObject> itemPrefabs;
    [SerializeField] private Sprite disabledSprite;
    [SerializeField] private MyItem item;

    public override void OnHit(bool popOut)
    {
        if (!isActive) return;
        Bounce();
        DisableTile(disabledSprite);
    }

    public override void SpawnItem()
    {
        GameObject _item;
        switch (item)
        {
            case MyItem.Coin:
                _item = Instantiate(itemPrefabs[0]);
                _item.GetComponent<Misc_Base>().Configure(transform.position);
                MainManager.instance.AddCoin(false);
                FloatingScorePool.instance.GetFromPool(transform.position, 200);
                break;
            case MyItem.PowerUp:
                if (Player.instance.GetCurrentPower() > 1)
                {
                    _item = Instantiate(itemPrefabs[2]);
                    _item.GetComponent<Misc_Base>().Configure(transform.position);
                }
                else
                {
                    _item = Instantiate(itemPrefabs[1]);
                    _item.GetComponent<Misc_Base>().Configure(transform.position);
                }
                break;
            case MyItem.Star:
                _item = Instantiate(itemPrefabs[3]);
                _item.GetComponent<Misc_Base>().Configure(transform.position);
                break;
        }
    }
}
