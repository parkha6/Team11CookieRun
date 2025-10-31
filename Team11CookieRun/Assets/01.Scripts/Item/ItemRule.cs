using UnityEngine;

[System.Serializable]
public class ItemRule
{
    public Item.ItemType itemType = Item.ItemType.Heal;
    [Range(0f, 1f)] public float spawnChance = 0.5f;
}
