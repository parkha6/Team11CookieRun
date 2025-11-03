using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemPrefab
{
    public Item.ItemType type;
    public GameObject prefab;

    public Item.CoinSubType coinSubType = Item.CoinSubType.Normal;
    public bool IsValid()
    {
        return prefab != null;
    }
}
