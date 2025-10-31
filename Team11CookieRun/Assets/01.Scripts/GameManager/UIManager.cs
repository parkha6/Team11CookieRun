using UnityEngine;
public class UIManager : SingletonManager<UIManager>//UI에 표시되는 변수와 관련되어있는 클래스
{
    #region Scores
    float score = 0; //점수
    internal float Score { get { return score; }set { score = value; } }
    float highScore = 0;//최고 점수 
    internal float HighScore { get { return highScore; }set { highScore = value; } }

    internal void ResetScore()
    {
        score = 0;
        currentHp = Hp;
    }
    #endregion
    #region Hp
    float hp = 100;
    internal float Hp
    {
        get { return hp; }
        private set
        {
            if (value <= GmConst.minHp)
            { value = GmConst.minHp; }
            hp = value;
        }
    }
    float currentHp = 100;//현재 Hp
    internal float CurrentHp
    {
        get { return currentHp; }
        set
        {
            if (value <= GmConst.minHp)
            { value = GmConst.minHp; }
            else if (value > hp)
            { value = hp; }
            currentHp = value;
        }
    }
    internal void SetHp(int getAmount)//음수를 넣으면 데미지 아닐까?
    {
        currentHp += getAmount;
        PlayerPrefs.SetFloat(GmConst.hpKey, CurrentHp);
    }
    internal bool IsDead()
    { return currentHp <= GmConst.dead; }
    #endregion
    internal void LoadKey()//
    {
        if (PlayerPrefs.HasKey(GmConst.highScoreKey))
        { highScore = PlayerPrefs.GetFloat(GmConst.highScoreKey, 0); }
        if (PlayerPrefs.HasKey(GmConst.hpKey))
        { currentHp = PlayerPrefs.GetFloat(GmConst.hpKey, 100); }
    }

}
