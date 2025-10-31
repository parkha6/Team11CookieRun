using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : SingletonManager<UIManager>//UI에 표시되는 변수와 관련되어있는 클래스
{//게임매니저란 무엇인가...
    const string highScoreKey = "High Score";
    const string hpKey = "Current Hp";
    const byte dead = 0;
    const byte minScore = 0;
    const byte minHp = 0;
    [SerializeField]
    GameObject EndUi;
    [SerializeField]
    GameObject PauseUi;
 
    [SerializeField]
    GameObject star;
    [SerializeField]
    GameObject newText;
    #region Scores
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI finalScoreText;
    [SerializeField]
    TextMeshProUGUI highscoreText;
    float score = 0; //점수
    float highScore = 0;//최고 점수 
    internal void SetScore(int getAmount)
    {
        score += getAmount;
        scoreText.text = score.ToString();
    }
    internal void ShowScore()
    { scoreText.text = score.ToString(); }
    internal void CompareScore()//게임이 끝나면 쓰는 함수
    {
        finalScoreText.text = score.ToString();
        if (score > highScore || !PlayerPrefs.HasKey(highScoreKey))
        {
            PlayerPrefs.SetFloat(highScoreKey, score);
            star.SetActive(true);
            newText.SetActive(true);
        }
        highScore = PlayerPrefs.GetFloat(highScoreKey,minScore);
        highscoreText.text = highScore.ToString();
    }
    internal void ResetScore()
    {
        score = 0;
        currentHp = Hp;
    }
    #endregion
    #region Hp
    [SerializeField]
    Image hpBar;
    float hp = 100;
    internal float Hp
    {
        get { return hp; }
        private set
        {
            if (value <= minHp)
            { value = minHp; }
            hp = value;
        }
    }
    float currentHp = 100;//현재 Hp
    internal float CurrentHp
    {
        get { return currentHp; }
        set
        {
            if (value <= minHp)
            { value = minHp; }
            else if (value > hp)
            { value = hp; }
            currentHp = value;
        }
    }
    internal void ShowHp()
    { hpBar.fillAmount = CurrentHp / Hp; }
    internal void SetHp(int getAmount)//음수를 넣으면 데미지 아닐까?
    {
        currentHp += getAmount;
        PlayerPrefs.SetFloat(hpKey, CurrentHp);
    }
    internal bool IsDead()
    { return currentHp <= dead; }
    #endregion
    internal void LoadKey()//
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        { highScore = PlayerPrefs.GetFloat(highScoreKey, 0); }
        if (PlayerPrefs.HasKey(hpKey))
        { currentHp = PlayerPrefs.GetFloat(hpKey, 100); }
    }
    internal void HideStar()
    {
        if (star.activeInHierarchy)
        { star.SetActive(false); }
        if (newText.activeInHierarchy)
        { newText.SetActive(false); }
    }
    internal void ShowPauseUI()
    { PauseUi.SetActive(true); }
    internal void ShowEndUI()
    { EndUi.SetActive(true); }
    internal void HideUi()//UI숨김처리
    {
        HideStar();
        if (PauseUi.activeInHierarchy)
        { PauseUi.SetActive(false); }
        if (EndUi.activeInHierarchy)
        { EndUi.SetActive(false); }
    }
}
