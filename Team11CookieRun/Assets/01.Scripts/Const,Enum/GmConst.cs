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
