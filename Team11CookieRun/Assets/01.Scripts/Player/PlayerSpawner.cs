using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameUIManager gameCanvasManager;
    [SerializeField] FollowCamera camera;
    [SerializeField] MapPieceManager mapPieceManager;
    [SerializeField] Vector3 startPos;
    void Awake()
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
        mapPieceManager._playerTransform = player.transform;
    }
}
