using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] StartCanvasManager gameCanvasManager;
    [SerializeField] FollowCamera camera;
    [SerializeField] Vector3 startPos;
    void Start()
    {
        GameObject go = Instantiate(playerPrefab, startPos, Quaternion.identity);
        if (go.GetComponent<Player>())
        {
            Player player = go.GetComponent<Player>();
            SettingGame(player);
        }
    }

    private void SettingGame(Player player)
    {
        player.InitCanvasManager(gameCanvasManager);
        camera.target = player.transform;
        MapPieceManager.Instance._playerTransform = player.transform;
    }
}
