using UnityEngine;
public class ScoreManager : SingletonManager<ScoreManager>//UI에 표시되는 변수와 관련되어있는 클래스
{

    float highScore = 0;//최고 점수 
    internal float HighScore { get { return highScore; } set { highScore = value; } }
    /// <summary>
    /// 점수 재설정을 위해 최소 점수값 0을 반환하는 메서드.
    /// </summary>
    /// <returns></returns>
    internal float ResetScore()
    { return GmConst.minScore; }
    /// <summary>
    /// 저장된 최대 점수가 존재하면 불러오는 변수.
    /// </summary>
    internal void LoadKey()//
    {
        if (PlayerPrefs.HasKey(GmConst.highScoreKey))
        { highScore = PlayerPrefs.GetFloat(GmConst.highScoreKey, 0); }
    }
}
