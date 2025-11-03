using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 매니저쪽에서 사용하는 영구적인 변수값
/// 하나 빼곤 다 매직넘버 방지용.
/// </summary>
public class GmConst
{
    /// <summary>
    /// 최대점수의 PlayerPrefs string키
    /// </summary>
    internal const string highScoreKey = "High Score";
    /// <summary>
    /// 메인화면의 최대 화면비
    /// </summary>
    internal const float mainMenuMaxRatio = 341f / 752f;
    /// <summary>
    /// 게임화면의 최대 가로 화면비
    /// </summary>
    internal const float gameSceneMaxWideRatio = 1210f / 144f;
    /// <summary>
    /// 게임화면의 최대 세로 화면비
    /// </summary>
    internal const float gameSceneMaxHeightRatio = 192f / 647f;
    /// <summary>
    /// Expand가 아닐시 메인화면의 매치 비율.
    /// </summary>
    internal const float mainMenuMatchRatio = 0.863f;
    /// <summary>
    /// Expand가 아닐시 게임화면의 매치 비율.
    /// </summary>
    internal const float gameSceneMatchRatio = 0.367f;
    /// <summary>
    /// 최대 비율
    /// </summary>
    internal const byte maxRatio = 1;
    /// <summary>
    /// 최소 비율
    /// </summary>
    internal const byte minRatio = 0;
    /// <summary>
    /// 최소점수
    /// </summary>
    internal const byte minScore = 0;
    /// <summary>
    /// Hp값의 절반비율
    /// </summary>
    internal const float halfHp = 0.5f;
    /// <summary>
    /// 죽은 상태의 숫자 값
    /// </summary>
    internal const byte dead = 0;
    /// <summary>
    /// 시간이 멈추게 되는 숫자 값
    /// </summary>
    internal const byte stopTime = 0;
    /// <summary>
    /// 시간이 재생되는 숫자 값
    /// </summary>
    internal const byte runTime = 1;
}
