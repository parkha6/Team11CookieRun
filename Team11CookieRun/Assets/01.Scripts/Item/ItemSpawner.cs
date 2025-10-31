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

    private Dictionary<Item.ItemType, Queue<GameObject>> itemPool = new Dictionary<Item.ItemType, Queue<GameObject>>();

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

        foreach(var entry in itemPrefabs)
        {
            if (!itemPool.ContainsKey(entry.type))
            {
                itemPool.Add(entry.type, new Queue<GameObject>());
            }

            for (int i = 0; i < initialPoolSizePerType; i++)
            {
                GameObject obj = Instantiate(entry.prefab, transform);

                obj.SetActive(false);
                itemPool[entry.type].Enqueue(obj);
            }
        }
    }

    // 아이템을 풀에서 가져옴
    public GameObject GetItem(Item.ItemType type, Vector3 Position)
    {
        GameObject item = null;

        if (itemPool.ContainsKey(type) && itemPool[type].Count > 0)
        {
            item = itemPool[type].Dequeue();
        }
        else
        {
            ItemPrefab entry = itemPrefabs.FirstOrDefault(e => e.type == type && e.IsValid());

            if (entry == null)
            {
                return null;
            }
            item = Instantiate(entry.prefab, transform);
        }

        if (item != null)
        {
            item.transform.position = Position;
            item.transform.rotation = Quaternion.identity;
            item.SetActive(true);
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

        if (itemPool.ContainsKey(type))
        {
            itemPool[type].Enqueue(item);
        }
        else
        {
            Destroy(item);
        }
    }

    // 아이템 풀을 초기화 (게임 리셋)
    public void ResetAllItems()
    {
        foreach(var pool in itemPool.Values)
        {
            while(pool.Count > 0)
            {
                pool.Dequeue().gameObject.SetActive(false);
            }
        }

        itemPool.Clear();
        InitializePool();
    }
}
