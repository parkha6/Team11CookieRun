using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
public class StartCanvasManager : MonoBehaviour
{
    GameManager gameManager;
    UIManager uiManager;
    private void Awake()//시작시점에 필요한 변수를 로드하게 만들었음.
    { 
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        gameManager.AddStartScene(this);
        OnClickAddListeners();
    }
    #region debugUI
    [SerializeField]
    internal GameObject debugUI;
    [SerializeField]
    internal Button deleteDataButton;
    #endregion
    #region DefaultUI
    [SerializeField]
    internal string sceneName;
    [SerializeField]
    internal string homeSceneName;
    [SerializeField]
    internal Image hpBar;
    [SerializeField]
    internal TextMeshProUGUI scoreText;
    #endregion
    #region PauseUI
    [SerializeField]
    internal GameObject PauseUi;
    [SerializeField]
    internal Button pauseOptionButton;
    [SerializeField]
    internal Button pauseHomeButton;
    [SerializeField]
    internal Button pauseSettingButton;
    [SerializeField]
    internal Button pauseExitButton;
    [SerializeField]
    internal Button homeButton;
    [SerializeField]
    internal Button restartButton;
    #endregion
    #region EndUI
    [SerializeField]
    internal GameObject EndUi;
    [SerializeField]
    internal TextMeshProUGUI finalScoreText;
    [SerializeField]
    internal TextMeshProUGUI highscoreText;
    [SerializeField]
    internal GameObject star;
    [SerializeField]
    internal GameObject newText;
    #endregion
    void OnClickAddListeners()
    {
        if (pauseOptionButton != null)
        { pauseOptionButton.onClick.AddListener(gameManager.OnClickGamePause); }
        if (pauseHomeButton != null)
        { pauseHomeButton.onClick.AddListener(OnClickHome); }
        if (pauseExitButton != null)
        { pauseExitButton.onClick.AddListener(gameManager.OnClickExitPause); }
        if (homeButton != null)
        { homeButton.onClick.AddListener(OnClickHome); }
        if (restartButton != null)
        { restartButton.onClick.AddListener(Restart); }
        if (deleteDataButton != null)
        { deleteDataButton.onClick.AddListener(gameManager.DeleteData); }
        if (deleteDataButton != null)
        {
            deleteDataButton.onClick.AddListener(gameManager.DeleteData);
            if (gameManager.debugMode)
            { debugUI.SetActive(true); }
        }
    }
    void Restart()
    { gameManager.OnClickRestart(sceneName); }
    internal void SetScore(int getAmount)
    {
        uiManager.Score += getAmount;
        scoreText.text = uiManager.Score.ToString();
    }
    internal void ShowScore()
    { scoreText.text = uiManager.Score.ToString(); }
    internal void CompareScore()//게임이 끝나면 쓰는 함수
    {
        finalScoreText.text = uiManager.Score.ToString();
        if (uiManager.Score > uiManager.HighScore || !PlayerPrefs.HasKey(GmConst.highScoreKey))
        {
            PlayerPrefs.SetFloat(GmConst.highScoreKey, uiManager.Score);
            star.SetActive(true);
            newText.SetActive(true);
        }
        uiManager.HighScore = PlayerPrefs.GetFloat(GmConst.highScoreKey, GmConst.minScore);
        highscoreText.text = uiManager.HighScore.ToString();
    }
    internal void ShowHp()
    { hpBar.fillAmount = uiManager.CurrentHp / uiManager.Hp; }
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
    internal void OnClickHome()
    {
        if (gameManager.currentStage == GameStage.End)
        {
            //isEnd = false;
            gameManager.SaveGame();
            uiManager.ResetScore();
            HideUi();
            gameManager.MoveScene(homeSceneName);
        }
    }
}
