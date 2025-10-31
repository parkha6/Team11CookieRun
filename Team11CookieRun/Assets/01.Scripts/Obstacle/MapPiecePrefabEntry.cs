using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapPiecePrefabEntry
{
    // 엔트리가 나타내는 맵 조각의 타입
    public MapType type = MapType.Forest_Flat;

    // 타입에 해당하는 실제 맵 조각 게임 오브젝트(프리팹)
    public GameObject prefab;

    // 맵 생성 시 이 조각이 선택될 확률의 가중치 (높을수록 자주 선택)
    [Range(0.1f, 10f)]
    public float weight = 1.0f;

    public bool IsValid()
    {
        return prefab != null;
    }
}
