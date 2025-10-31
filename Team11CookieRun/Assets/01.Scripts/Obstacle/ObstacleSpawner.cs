using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class ObstacleSpawner : MonoBehaviour
{
    // 싱글턴
    public static ObstacleSpawner Instance;

    // 각 타입별 장애물 프리팹 목록
    [SerializeField] private List<ObstaclePrefab> _obstaclePrefabEntries;

    // 각 장애물 종류별로 게임 시작 시 미리 만들어 놓을 장애물의 개수
    [SerializeField] private int _initialPoolSizePerType = 5;

    // 장애물 종류별로 빈 게임 오브젝트들을 담아두는 창고
    private Dictionary<ObstacleType, Queue<GameObject>> _obstaclePool = new Dictionary<ObstacleType, Queue<GameObject>>();

    void Awake()
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
        if (_obstaclePrefabEntries == null || _obstaclePrefabEntries.Count == 0)
        {
            Debug.LogError("장애물 프리팹 목록이 비어있어 장애물을 생성할 수 없습니다.", this);
            return;
        }

        // 모든 프리팹 엔트리를 하나씩 확인
        foreach (var entry in _obstaclePrefabEntries) 
        {
            // 엔트리 정보가 제대로 되어있는지 확인
            if (entry == null || !entry.IsValid()) 
            {
                Debug.LogWarning($"유효하지 않은 장애물 프리팹 엔트리가 있습니다. (null 또는 프리팹 없음)(타입: {entry?.type})", this);
                continue;
            }

            // 해당 장애물 타입(entry.type)을 위한 창고(Queue)가 없으면 새로 만듬
            if (!_obstaclePool.ContainsKey(entry.type))
            {
                _obstaclePool.Add(entry.type, new Queue<GameObject>());
            }

            // _initialPoolSizePerType 개수만큼 장애물 오브젝트를 미리 만들어서 창고에 넣음
            for (int i = 0; i < _initialPoolSizePerType; i++)
            {
                GameObject obj = Instantiate(entry.prefab, transform); // 프리팹을 만들고, 이 스포너의 자식으로
                obj.SetActive(false); // 처음에는 눈에 보이지 않게 비활성화 상태로
                _obstaclePool[entry.type].Enqueue(obj); // 창고에 넣음
            }
        }
    }

    public GameObject GetObstacle(ObstacleType type, Vector3 position)
    {
        GameObject obstacleToUse = null;

        // 해당 타입의 장애물 창고가 있고, 안에 장애물이 있으면 꺼내 사용
        if (_obstaclePool.ContainsKey(type) && _obstaclePool[type].Count > 0)
        {
            obstacleToUse = _obstaclePool[type].Dequeue();
        }
        else // 창고에 장애물이 없으면 새로 생성
        {
            // 해당 타입의 프리팹 엔트리를 찾음
            ObstaclePrefab entry = _obstaclePrefabEntries.FirstOrDefault(e => e.type == type && e.IsValid());
            if (entry == null)
            {
                return null;
            }
            obstacleToUse = Instantiate(entry.prefab, transform); // 새로 만들고 스포너의 자식으로
        }

        if (obstacleToUse != null)
        {
            obstacleToUse.transform.position = position; // 원하는 위치에 배치
            obstacleToUse.transform.rotation = Quaternion.identity; // 회전 초기화
            obstacleToUse.SetActive(true); // 눈에 보이도록 활성화
        }
        return obstacleToUse;
    }

    public void ReturnObstacle(GameObject obstacle, ObstacleType type)
    {
        if (obstacle == null)
        {
            return;
        }

        obstacle.SetActive(false); // 눈에 보이지 않게 비활성화
        obstacle.transform.SetParent(transform); // 스포너의 자식으로 다시 정리

        if (_obstaclePool.ContainsKey(type))
        {
            _obstaclePool[type].Enqueue(obstacle); // 해당 타입의 창고에 다시 넣음
        }
        /*else
        {
            Destroy(obstacle);
        }*/
    }

    // 게임을 초기상태로 되돌림
    public void ResetAllObstacles()
    {
        foreach (var pool in _obstaclePool.Values)
        {
            while (pool.Count > 0)
            {
                Destroy(pool.Dequeue());
            }
            pool.Clear();
        }
        _obstaclePool.Clear();

        InitializePool();
    }
}