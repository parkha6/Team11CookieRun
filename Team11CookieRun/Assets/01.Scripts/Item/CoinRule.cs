using UnityEngine;

[System.Serializable]
public class CoinRule
{
    public Item.CoinSubType CoinSubType = Item.CoinSubType.Normal;
    [Range(0f, 1f)] public float spawnChance = 1f;
}
