using UnityEngine;
public class UIManager : SingletonManager<UIManager>//UI에 표시되는 변수와 관련되어있는 클래스
{
    float highScore = 0;//최고 점수 
    internal float HighScore { get { return highScore; } set { highScore = value; } }
    internal float ResetScore()
    { return GmConst.minScore; }
    internal void LoadKey()//
    {
        if (PlayerPrefs.HasKey(GmConst.highScoreKey))
        { highScore = PlayerPrefs.GetFloat(GmConst.highScoreKey, 0); }
    }

}
