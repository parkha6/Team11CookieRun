using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;

    // 아이템 프리팹 목록
    [SerializeField] private List<ItemPrefab> itemPrefabs;

    // 각 아이템을 종류별로 미리 만들어 놓을 개수
    [SerializeField] private int initialPoolSizePerType = 20;

    // private Dictionary<Item.ItemType, Queue<GameObject>> itemPool = new Dictionary<Item.ItemType, Queue<GameObject>>();
    private Dictionary<Item.ItemType, Dictionary<Item.CoinSubType, Queue<GameObject>>> itemPool = new Dictionary<Item.ItemType, Dictionary<Item.CoinSubType, Queue<GameObject>>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            InitializePool();
        }
    }

    private void InitializePool()
    {
        if (itemPool == null || itemPool.Count == 0)
        {
            return;
        }

        foreach (var typePool in itemPool.Values)
        {
            foreach ( var subTypePool in typePool.Values)
            {
                while(subTypePool.Count > 0)
                {
                    GameObject obj = subTypePool.Dequeue();
                    if(obj != null)
                    {
                        Destroy(obj);
                    }
                }
            }
        }
        itemPool.Clear();

        foreach(var entry in itemPrefabs)
        {
            if (!itemPool.ContainsKey(entry.type))
            {
                itemPool.Add(entry.type, new Dictionary<Item.CoinSubType, Queue<GameObject>>());
            }

            Item.CoinSubType currentCoinSubType = entry.coinSubType;

            if(entry.type != Item.ItemType.Coin)
            {
                currentCoinSubType = Item.CoinSubType.Normal;
            }

            if (!itemPool[entry.type].ContainsKey(currentCoinSubType))
            {
                itemPool[entry.type].Add(currentCoinSubType, new Queue<GameObject>());
            }

            Queue<GameObject> targetQueue = itemPool[entry.type][currentCoinSubType];

            for (int i = 0; i < initialPoolSizePerType; i++)
            {
                GameObject obj = Instantiate(entry.prefab, transform);

                Item itemComponent = obj.GetComponent<Item>();
                if (itemComponent != null)
                {
                    itemComponent.itemType = entry.type;
                        if (entry.type == Item.ItemType.Coin)
                    {
                        itemComponent.coinSubType = entry.coinSubType;
                    }
                }

                obj.SetActive(false);
                targetQueue.Enqueue(obj);
            }
        }
    }

    // 아이템을 풀에서 가져옴
    public GameObject GetItem(Item.ItemType type, Vector3 position)
    {
        if (type == Item.ItemType.Coin)
        {
            return null;
        }

        return GetItemInternal(type, Item.CoinSubType.Normal, position);
    }

    public GameObject GetItem(Item.ItemType type, Item.CoinSubType coinSubType, Vector3 position)
    {
        return GetItemInternal(type, coinSubType, position);
    }

    private GameObject GetItemInternal(Item.ItemType type, Item.CoinSubType coinSubType, Vector3 position)
    {
        GameObject item = null;
        Queue<GameObject> targetQueue = null;

        if (itemPool.ContainsKey(type))
        {
            Item.CoinSubType targetCoinSubType = (type == Item.ItemType.Coin) ? coinSubType : Item.CoinSubType.Normal;

            if (itemPool[type].ContainsKey(targetCoinSubType))
            {
                targetQueue = itemPool[type][targetCoinSubType];
            }
        }

        if(targetQueue != null && targetQueue.Count > 0)
        {
            item = targetQueue.Dequeue();
        }
        else
        {
            ItemPrefab entry = itemPrefabs.FirstOrDefault(e =>
            e.type == type && ((type == Item.ItemType.Coin && e.coinSubType == coinSubType) ||
            (type != Item.ItemType.Coin && e.coinSubType == Item.CoinSubType.Normal)) && e.IsValid()
            );

            if (entry == null)
            {
                return null;
            }
            item = Instantiate(entry.prefab, transform);

            Item itemComponent = item.GetComponent<Item>();
            if(itemComponent != null)
            {
                itemComponent.itemType = entry.type;
                if(entry.type == Item.ItemType.Coin)
                {
                    itemComponent.coinSubType = entry.coinSubType;
                }
            }
        }

        if(item != null)
        {
            item.transform.position = position;
            item.transform.rotation = Quaternion.identity;
            item.SetActive(true);

            Item itemComponent = item.GetComponent<Item>();
            if(itemComponent != null)
            {
                itemComponent.itemType = type;
                if(type == Item.ItemType.Coin)
                {
                    itemComponent.coinSubType = coinSubType;
                }
            }
        }

        return item;
    }

    // 아이템을 풀로 반환
    public void ReturnItem(GameObject item, Item.ItemType type)
    {
        if (item == null)
        {
            return;
        }

        item.SetActive(false);
        item.transform.SetParent(transform);

        Item itemComponent = item.GetComponent<Item>();

        if(itemComponent == null)
        {
            Destroy(item);
            return;
        }

        Item.ItemType returnItemType = itemComponent.itemType;
        Item.CoinSubType returnCoinSubType = itemComponent.coinSubType;

        if (itemPool.ContainsKey(returnItemType))
        {
            Item.CoinSubType targetCoinSubType = (returnItemType == Item.ItemType.Coin) ? returnCoinSubType : Item.CoinSubType.Normal;

            if (itemPool[returnItemType].ContainsKey(targetCoinSubType))
            {
                itemPool[returnItemType][targetCoinSubType].Enqueue(item);
            }
            else
            {
                Destroy(item);
            }
        }
        else
        {
            Destroy(item);
        }
    }

    // 아이템 풀을 초기화 (게임 리셋)
    public void ResetAllItems()
    {
        foreach(var typePool in itemPool.Values)
        {
            foreach (var subTypePool in typePool.Values)
            {
                while(subTypePool.Count > 0)
                {
                    GameObject obj = subTypePool.Dequeue();
                    if (obj != null)
                    {
                        Destroy(obj);
                    }
                }
            }
        }

        itemPool.Clear();
        InitializePool();
    }
}
