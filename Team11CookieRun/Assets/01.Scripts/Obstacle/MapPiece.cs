using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapPiece : MonoBehaviour
{
    public MapType mapType = MapType.Forest_Flat;

    // 조각의 시작 지점(이전 조각의 끝점에 연결)을 나타내는 Transform
    [SerializeField] private Transform _pieceStartPoint;

    // 조각의 끝 지점(다음 조각의 시작점이 여기에 연결됨)을 나타내는 Transform
    [SerializeField] private Transform _pieceEndPoint;

    // 이 조각 내에서 장애물을 배치할 수 있는 지점들의 Transform 리스트
    [SerializeField] private List<Transform> _obstacleSpawnPoints;

    // 이 맵 조각에 적용될 장애물 생성 규칙들의 리스트
    [SerializeField] private List<ObstacleRule> _obstacleRules;

    // 현재 맵 조각에 의해 생성된 장애물들을 추적하는 리스트
    private List<GameObject> _spawnedObstacles = new List<GameObject>();

    // 맵 조각의 시작 위치를 제공 (World Space)
    public Vector3 StartPosition
    {
        get { return _pieceStartPoint != null ? _pieceStartPoint.position : transform.position; }
    }

    // 맵 조각의 끝 위치를 제공 (World Space). 다음 조각이 여기에 연결
    public Vector3 EndPosition
    {
        get
        {
            // _pieceEndPoint 할당되지 않았다면, 현재 위치 기준으로 오른쪽으로 일정 거리 반환
            return _pieceEndPoint != null ? _pieceEndPoint.position : transform.position + Vector3.right * 10f;
        }
    }

    public void InitializePiece()
    {
        // 기존에 생성했던 장애물들을 먼저 풀로 반환하여 정리
        ClearSpawnedObstacles();

        // 자신이 가진 _obstacleRules에 따라 장애물을 생성
        if (_obstacleRules != null && ObstacleSpawner.Instance != null)
        {
            foreach (var rule in _obstacleRules)
            {
                GenerateObstaclesBasedOnRule(rule);
            }
        }
        else
        {
            Debug.LogWarning($"MapSegment '{gameObject.name}' 초기화에 실패했습니다. 유효한 _obstacleRules가 없거나 ObstacleSpawner가 없습니다.", this);
        }
    }

    // 특정 장애물 생성 규칙에 따라 이 세그먼트 내에 장애물을 생성하는 함수
    private void GenerateObstaclesBasedOnRule(ObstacleRule rule)
    {
        // 규칙이 유효하지 않거나 스폰 지점이 없으면 진행하지 않음
        if (rule == null || _obstacleSpawnPoints == null || !_obstacleSpawnPoints.Any())
        {
            return;
        }

        // 규칙에 정의된 최소, 최대 개수 내에서 랜덤으로 생성할 장애물 개수를 결정
        int obstaclesToSpawnCount = Random.Range(rule.minObstaclesToSpawn, rule.maxObstaclesToSpawn + 1);
        List<Transform> availableSpawnPoints = new List<Transform>(_obstacleSpawnPoints); // 사용 가능한 스폰 포인트를 복사

        for (int i = 0; i < obstaclesToSpawnCount; i++)
        {
            if (Random.value < rule.spawnChance) // 확률에 따라 장애물 생성 여부를 결정
            {
                // 사용 가능한 스폰 포인트를 모두 사용했다면 더 이상 생성하지 않음
                if (availableSpawnPoints.Count == 0) break;

                // 랜덤으로 스폰 지점 선택 후, 사용한 스폰 포인트는 리스트에서 제거하여 중복 사용 방지
                int spawnPointIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[spawnPointIndex];
                availableSpawnPoints.RemoveAt(spawnPointIndex);

                // ObstacleSpawner를 통해 장애물을 가져오고 배치
                GameObject obstacleObj = ObstacleSpawner.Instance.GetObstacle(rule.obstacleType, spawnPoint.position);
                if (obstacleObj != null)
                {
                    obstacleObj.transform.SetParent(spawnPoint);
                    obstacleObj.transform.localPosition = Vector3.zero;
                    obstacleObj.transform.localRotation = Quaternion.identity;

                    _spawnedObstacles.Add(obstacleObj); // 생성된 장애물을 리스트에 추가
                }
            }
        }
    }

   
    // 이 조각에서 생성된 모든 장애물들을 풀에 반환하고 리스트를 비움
    private void ClearSpawnedObstacles()
    {
        if (ObstacleSpawner.Instance == null) return;

        // 생성된 장애물들을 ObstacleSpawner를 통해 풀로 반환
        foreach (GameObject obstacle in _spawnedObstacles)
        {
            if (obstacle != null) // 혹시 이미 파괴되었을 경우를 대비한 Null 체크
            {
                Obstacle item = obstacle.GetComponent<Obstacle>();
                if (item != null)
                {
                    ObstacleSpawner.Instance.ReturnObstacle(obstacle, item.type);
                }
                else
                {
                    Destroy(obstacle);
                }
            }
        }
        _spawnedObstacles.Clear();
    }

    // 오브젝트 풀로 반환될 때 장애물을 정리
    void OnDisable()
    {
        ClearSpawnedObstacles();
    }
}
