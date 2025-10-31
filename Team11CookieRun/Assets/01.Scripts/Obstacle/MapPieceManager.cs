using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MapPieceManager : MonoBehaviour
{
    public static MapPieceManager Instance;

    // 게임에 등장할 맵 조각들의 타입과 그에 해당하는 프리팹들을 여기에 연결
    [SerializeField] private List<MapPiecePrefabEntry> _mapPiecePrefabEntries;

    // 추적할 플레이어의 Transform 맵 생성 기준점
    [SerializeField] private Transform _playerTransform;

    // 게임 시작 시 미리 생성할 맵 조각 개수
    [SerializeField] private int _initialPieceCount = 5;

    // 플레이어로부터 이 거리만큼 앞에 맵 조각이 없으면 새로 생성
    [SerializeField] private float _pieceSpawnAheadDistance = 20f;

    // 플레이어로부터 이 거리만큼 뒤에 있는 맵 조각을 풀로 반환
    [SerializeField] private float _pieceDespawnBehindDistance = 20f;

    // 맵 생성을 시작할 좌표
    [SerializeField] private Vector3 _initialMapStartPoint = Vector3.zero;

    // 맵 조각의 EndPoint
    private Vector3 _nextSpawnPosition;

    // 현재 활성화되어 있는 맵 조각을 순서대로 관리하는 큐
    private Queue<MapPiece> _activePieces = new Queue<MapPiece>();

    // 맵 조각 오브젝트 풀
    private Dictionary<MapType, Queue<MapPiece>> _piecePool = new Dictionary<MapType, Queue<MapPiece>>();



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

    void Start()
    {
        if (_playerTransform == null)
        {
            Debug.LogError("MapManager에 _playerTransform이 할당되지 않았습니다.", this);
            enabled = false;
            return;
        }

        _nextSpawnPosition = _initialMapStartPoint;

        // 초기 맵 조각들을 생성
        for (int i = 0; i < _initialPieceCount; i++)
        {
            GenerateNextPiece();
        }
    }

    void Update()
    {
        // 플레이어가 일정 거리 이상 전진하면 새로운 맵 조각을 생성
        // 현재 플레이어 위치 + (앞으로 생성될 거리)가 다음 생성 위치보다 크면 생성
        if (_playerTransform.position.x + _pieceSpawnAheadDistance > _nextSpawnPosition.x)
        {
            GenerateNextPiece();
        }

        // 플레이어 뒤로 멀리 지나간 맵 조각은 재활용 풀에 반환
        // 활성화된 조각이 있고, 플레이어 위치 - (뒤로 지나간 거리)가 가장 오래된 조각의 끝 지점보다 크면 반환
        if (_activePieces.Count > 0 &&
            _playerTransform.position.x - _pieceDespawnBehindDistance > _activePieces.Peek().EndPosition.x)
        {
            RecycleOldestPiece();
        }
    }

    private void InitializePool()
    {
        if (_mapPiecePrefabEntries == null || _mapPiecePrefabEntries.Count == 0)
        {
            Debug.LogError("맵 조각 프리팹 목록이 비어있습니다.", this);
            return;
        }

        foreach (var entry in _mapPiecePrefabEntries)
        {
            if (!_piecePool.ContainsKey(entry.type))
            {
                _piecePool.Add(entry.type, new Queue<MapPiece>());
            }
        }
    }

    private MapPiece GetPieceFromPool(MapType type)
    {
        MapPiece PieceToUse = null;

        if (_piecePool.ContainsKey(type) && _piecePool[type].Count > 0)
        {
            PieceToUse = _piecePool[type].Dequeue();
        }
        else // 풀에 해당 타입의 오브젝트가 없으면 새로 생성
        {
            MapPiecePrefabEntry entry = _mapPiecePrefabEntries.FirstOrDefault(e => e.type == type && e.IsValid());
            if (entry == null)
            {
                return null;
            }

            GameObject obj = Instantiate(entry.prefab, transform);
            PieceToUse = obj.GetComponent<MapPiece>();
            if (PieceToUse == null)
            {
                Destroy(obj);
                return null;
            }
        }

        if (PieceToUse != null)
        {
            PieceToUse.gameObject.SetActive(true);
        }
        return PieceToUse;
    }

    private void ReturnPieceToPool(MapPiece piece)
    {

        MapType type = piece.mapType;

        piece.gameObject.SetActive(false);
        piece.transform.SetParent(transform);

        if (_piecePool.ContainsKey(type))
        {
            _piecePool[type].Enqueue(piece);
        }
        else
        {
            Destroy(piece.gameObject);
        }
    }

    private void GenerateNextPiece()
    {
        // 가중치를 고려하여 랜덤으로 선택 (난이도 등에 따라 선택 로직 변경 가능)
        MapType nextPieceType = SelectPieceTypeByWeightedRandom();
        if (nextPieceType == MapType.Forest_Flat && _mapPiecePrefabEntries.Count > 0 && _mapPiecePrefabEntries.All(e => e.type != MapType.Forest_Flat))
        {
            return;
        }


        MapPiece newPiece = GetPieceFromPool(nextPieceType);
        if (newPiece == null)
        {
            return;
        }
        
        newPiece.transform.position = _nextSpawnPosition;

        newPiece.InitializePiece();

        _activePieces.Enqueue(newPiece);

        _nextSpawnPosition = newPiece.EndPosition;
    }


    private void RecycleOldestPiece()
    {
        if (_activePieces.Count == 0) return;

        MapPiece pieceToRecycle = _activePieces.Dequeue();
        ReturnPieceToPool(pieceToRecycle);
    }


    private MapType SelectPieceTypeByWeightedRandom()
    {
        List<MapPiecePrefabEntry> validEntries = _mapPiecePrefabEntries
            .Where(entry => entry != null && entry.IsValid())
            .ToList();

        if (!validEntries.Any())
        {
            return MapType.Forest_Flat;
        }

        float totalWeight = validEntries.Sum(entry => entry.weight);

        float randomNumber = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var entry in validEntries)
        {
            currentWeight += entry.weight;
            if (randomNumber <= currentWeight)
            {
                return entry.type;
            }
        }

        return validEntries.Last().type;
    }


    public void ResetMap()
    {

        while (_activePieces.Count > 0)
        {
            ReturnPieceToPool(_activePieces.Dequeue());
        }

        foreach (var pool in _piecePool.Values)
        {
            while (pool.Count > 0)
            {
                pool.Dequeue().gameObject.SetActive(false);
            }
        }

        _nextSpawnPosition = _initialMapStartPoint;

        Start();
    }
}