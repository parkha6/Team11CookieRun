using UnityEngine;
public class UIManager : SingletonManager<UIManager>//UI에 표시되는 변수와 관련되어있는 클래스
{
    #region Scores
    float score = 0; //점수
    internal float Score { get { return score; }set { score = value; } }
    float highScore = 0;//최고 점수 
    internal float HighScore { get { return highScore; }set { highScore = value; } }

    internal float ResetScore()
    {
        score = 0;
        return 100f;//TODO:임시 회복임 고쳐야함.
    }
    #endregion
    internal void LoadKey()//
    {
        if (PlayerPrefs.HasKey(GmConst.highScoreKey))
        { highScore = PlayerPrefs.GetFloat(GmConst.highScoreKey, 0); }
    }

}
