using UnityEngine;
/// <summary>
/// 게임의 점수를 관리하는 클래스.
/// </summary>
public class ScoreManager : SingletonManager<ScoreManager>//UI에 표시되는 변수와 관련되어있는 클래스
{
    /// <summary>
    /// 최고점수를 불러와서 저장하는 구간.
    /// </summary>
    float highScore = 0;//최고 점수 
    /// <summary>
    /// 최고점수를 프로퍼티로 공개.
    /// </summary>
    internal float HighScore
    {
        get { return highScore; }
        set
        {
            if (highScore < GmConst.minScore)
            { value = GmConst.minScore; }
            highScore = value;
        }
    }
    /// <summary>
    /// 점수 재설정을 위해 최소 점수값 0을 반환하는 메서드.
    /// </summary>
    /// <returns></returns>
    internal float ResetScore()
    { return GmConst.minScore; }
    /// <summary>
    /// 저장된 최대 점수가 존재하면 불러오는 메서드
    internal void LoadKey()//
    {
        if (PlayerPrefs.HasKey(GmConst.highScoreKey))
        { highScore = PlayerPrefs.GetFloat(GmConst.highScoreKey, GmConst.minScore); }
    }
}
