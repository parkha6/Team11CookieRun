using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ObstacleRule
{
    [Range(0f, 1f)]
    public float spawnChance = 0.5f;

    public ObstacleType obstacleType = ObstacleType.Spike_Under; // 사용할 장애물의 타입

    
    public int minObstaclesToSpawn = 0; // 장애물 배치 최소 갯수
    public int maxObstaclesToSpawn = 1; // 장애물 배치 최대 갯수

    // 장애물의 높이나 x위치 오프셋 추가
    // public Vector2 spawnOffsetMinMax;
}
