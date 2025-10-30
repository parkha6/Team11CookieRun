using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : SingletonManager<UIManager>//UI에 표시되는 변수와 관련되어있는 클래스
{//게임매니저란 무엇인가...
    const string highScoreKey = "High Score";
    const string hpKey = "Current Hp";
    [SerializeField]
    Image hpBar;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI scoreText2;
    [SerializeField]
    TextMeshProUGUI highscoreText;

    float hp = 100;
    internal float Hp
    {
        get { return hp; }
        private set
        {
            if (value <= 0)
            { value = 0; }
            hp = value;
        }
    }
    float currentHp = 100;//현재 Hp
    internal float CurrentHp
    {
        get { return currentHp; }
        set
        {
            if (value <= 0)
            { value = 0; }
            else if (value > hp)
            { value = hp; }
            currentHp = value;
        }
    }
    float score = 0; //점수
    float highScore = 0;//최고 점수 
    //protected override void Awake()
    //{ hpBar = GetComponent<Image>(); }
    internal void LoadKey()//
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        { highScore = PlayerPrefs.GetFloat(highScoreKey, 0); }
        if (PlayerPrefs.HasKey(hpKey))
        { currentHp = PlayerPrefs.GetFloat(hpKey, 100); }
    }
    internal void ShowHp()
    { hpBar.fillAmount = CurrentHp / Hp; }
    internal void SetHp(int getAmount)//음수를 넣으면 데미지 아닐까?
    {
        currentHp += getAmount;
        PlayerPrefs.SetFloat(hpKey, CurrentHp);
    }
    internal void SetScore(int getAmount)
    {
        score += getAmount;
        scoreText.text = score.ToString();
    }
    internal void ShowScore()
    { scoreText.text = score.ToString(); }
    internal void CompareScore()//게임이 끝나면 쓰는 함수
    {
        scoreText2.text = score.ToString();
        if (score > highScore)
        { PlayerPrefs.SetFloat(highScoreKey, score); }
    }
    internal void SaveGame()//Save 메서드입니다. 저장이 필요한 구간에 가져다 쓰세요.
    { PlayerPrefs.Save(); }
}
