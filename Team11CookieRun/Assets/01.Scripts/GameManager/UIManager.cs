using UnityEngine;
public class UIManager : SingletonManager<UIManager>//UI에 표시되는 변수와 관련되어있는 클래스
{
    const string levelKey = "Level";
    const string highScoreKey = "High Score";
    const string expKey = "Current Exp";
    const string hpKey = "Current Hp";

    int playerLevel = 1; //플레이어 레벨
    internal int PlayerLevel
    {
        get { return playerLevel; }
        private set
        {
            if (value <= 0)
            { value = 0; }
            playerLevel = value;
        }
    }
    int Exp { get { return 100 * playerLevel; } }//총 경험치 계산
    int currentExp = 0;//현재 Exp
    internal int CurrentExp
    {
        get { return currentExp; }
        private set
        {
            if (value <= 0)
            { value = 0; }
            currentExp = value;
        }
    }
    int Hp { get { return 100 * playerLevel; } }//총 hp.
    int currentHp = 100;//현재 Hp
    internal int CurrentHp
    {
        get { return currentHp; }
        private set
        {
            if (value <= 0)
            { value = 0; }
            else if (value > Hp)
            { value = Hp; }
            currentHp = value;
        }
    }
    int score = 0; //점수
    int highScore = 0;//최고 점수 
    internal void LoadKey()//
    {
        if (PlayerPrefs.HasKey(levelKey))
        { PlayerLevel = PlayerPrefs.GetInt(levelKey, 0); }
        if (PlayerPrefs.HasKey(highScoreKey))
        { highScore = PlayerPrefs.GetInt(highScoreKey, 0); }
        if (PlayerPrefs.HasKey(expKey))
        { currentExp = PlayerPrefs.GetInt(expKey, 0); }
        if (PlayerPrefs.HasKey(hpKey))
        { currentHp = PlayerPrefs.GetInt(hpKey, 0); }
    }
    internal void CheckLevelUp()//레벨업 함수. int를 리턴함
    {
        if (CurrentExp >= Exp)
        {
            ++PlayerLevel;
            PlayerPrefs.SetInt(levelKey, PlayerLevel);
        }
    }
    internal void SetExp(int getAmount)//경험치 추가 함수
    {
        CurrentExp += getAmount;
        PlayerPrefs.SetInt(expKey, currentExp);
    }
    internal void SetHp(int getAmount)//음수를 넣으면 데미지 아닐까?
    {
        currentHp += getAmount;
        PlayerPrefs.SetInt(hpKey, currentHp);
    }
    internal void SetScore(int getAmount)
    { score += getAmount; }
    internal void CompareScore()//게임이 끝나면 쓰는 함수
    {
        if (score > highScore)
        { PlayerPrefs.SetInt(highScoreKey, score); }
    }//또 뭐 필요할까...
}
