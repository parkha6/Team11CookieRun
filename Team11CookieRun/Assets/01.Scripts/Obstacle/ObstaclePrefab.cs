using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstaclePrefab
{
    public ObstacleType type;

    public GameObject prefab;

    public bool IsValid()
    {
        return prefab != null;
    }
}
