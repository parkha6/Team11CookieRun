using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBgmManager : MonoBehaviour
{
    /// <summary>
    /// 게임화면의 오디오소스
    /// </summary>
    [Tooltip("게임화면의 오디오소스")]
    [SerializeField] internal AudioSource audioSource;
    /// <summary>
    /// 스테이지에 들어갈 배경음악
    /// </summary>
    [Tooltip("스테이지에 들어갈 음악")]
    [SerializeField] internal AudioClip stageBgm;
    /// <summary>
    /// 결과화면 배경음악
    /// </summary>
    [Tooltip("결과화면 배경음악")]
    [SerializeField] internal AudioClip resultBgm;
    /// <summary>
    /// 게임매니저를 넣는 변수
    /// </summary>
    GameManager gameManager;
    private void Awake()
    {
        gameManager = GameManager.Instance;
        gameManager.AddGameBGM(this);
    }
}
