using UnityEngine;
public class UIManager : SingletonManager<UIManager>//UI에 표시되는 변수와 관련되어있는 클래스
{
    const string highScoreKey = "High Score";
    const string hpKey = "Current Hp";
    const byte dead = 0;
    const byte minScore = 0;
    const byte minHp = 0;
    #region Scores
    float score = 0; //점수
    float highScore = 0;//최고 점수 

    WaitingCanvasManager waitingCanvasManager;
    StartCanvasManager startCanvasManager;
    protected override void Awake()//시작시점에 필요한 변수를 로드하게 만들었음.
    {
        waitingCanvasManager = WaitingCanvasManager.Instance;
        startCanvasManager = StartCanvasManager.Instance;
    }
    internal void SetScore(int getAmount)
    {
        score += getAmount;
        startCanvasManager.scoreText.text = score.ToString();
    }
    internal void ShowScore()
    { startCanvasManager.scoreText.text = score.ToString(); }
    internal void CompareScore()//게임이 끝나면 쓰는 함수
    {
        startCanvasManager.finalScoreText.text = score.ToString();
        if (score > highScore || !PlayerPrefs.HasKey(highScoreKey))
        {
            PlayerPrefs.SetFloat(highScoreKey, score);
            startCanvasManager.star.SetActive(true);
            startCanvasManager.newText.SetActive(true);
        }
        highScore = PlayerPrefs.GetFloat(highScoreKey,minScore);
        startCanvasManager.highscoreText.text = highScore.ToString();
    }
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
    { startCanvasManager.hpBar.fillAmount = CurrentHp / Hp; }
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
        if (startCanvasManager.star.activeInHierarchy)
        { startCanvasManager.star.SetActive(false); }
        if (startCanvasManager.newText.activeInHierarchy)
        { startCanvasManager.newText.SetActive(false); }
    }
    internal void ShowPauseUI()
    { startCanvasManager.PauseUi.SetActive(true); }
    internal void ShowEndUI()
    { startCanvasManager.EndUi.SetActive(true); }
    internal void HideUi()//UI숨김처리
    {
        HideStar();
        if (startCanvasManager.PauseUi.activeInHierarchy)
        { startCanvasManager.PauseUi.SetActive(false); }
        if (startCanvasManager.EndUi.activeInHierarchy)
        { startCanvasManager.EndUi.SetActive(false); }
    }
}
