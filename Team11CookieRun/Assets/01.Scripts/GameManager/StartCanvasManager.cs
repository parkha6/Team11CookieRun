using TMPro;
using UnityEngine;
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
        gameManager.currentStage = GameStage.Start;
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
    internal GameObject pauseUi;
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
    internal GameObject endUi;
    [SerializeField]
    internal TextMeshProUGUI finalScoreText;
    [SerializeField]
    internal TextMeshProUGUI highscoreText;
    [SerializeField]
    internal GameObject star;
    [SerializeField]
    internal GameObject newText;
    #endregion
    #region Mobile
    [SerializeField] Player player;
    //[SerializeField] private Button pauseOptionButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button slideButton;
    #endregion
    void OnClickAddListeners()
    {
        if (pauseOptionButton != null)
        { pauseOptionButton.onClick.AddListener(OnMobilePause); }
        if (pauseHomeButton != null)
        { pauseHomeButton.onClick.AddListener(OnClickHome); }
        if (pauseExitButton != null)
        { pauseExitButton.onClick.AddListener(gameManager.OnClickExitPause); }
        if (homeButton != null)
        { homeButton.onClick.AddListener(OnClickHome); }
        if (restartButton != null)
        { restartButton.onClick.AddListener(Restart); }
        if (deleteDataButton != null)
        {
            deleteDataButton.onClick.AddListener(gameManager.DeleteData);
            if (gameManager.debugMode)
            { debugUI.SetActive(true); }
        }
        if (jumpButton != null) { jumpButton.onClick.AddListener(OnPlayerJump); }
        if (slideButton != null) { jumpButton.onClick.AddListener(OnPlayerSlide); }
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
    internal void ShowHp(float currentHp,float hp)
    { hpBar.fillAmount = currentHp / hp; }
    internal void HideStar()
    {
        if (star.activeInHierarchy)
        { star.SetActive(false); }
        if (newText.activeInHierarchy)
        { newText.SetActive(false); }
    }
    internal void ShowPauseUI()
    { pauseUi.SetActive(true); }
    internal void ShowEndUI()
    { endUi.SetActive(true); }
    internal void HideUi()//UI숨김처리
    {
        HideStar();
        if (pauseUi.activeInHierarchy)
        { pauseUi.SetActive(false); }
        if (endUi.activeInHierarchy)
        { endUi.SetActive(false); }
    }
    internal void OnClickHome()
    {
        if (gameManager.currentStage == GameStage.End)
        {
            //isEnd = false;
            gameManager.SaveGame();
            uiManager.ResetScore();
            HideUi();
            gameManager.currentStage = GameStage.Waiting;
            gameManager.MoveScene(homeSceneName);
        }
    }

    private void OnPlayerJump()
    {
        if (player == null) return;
        player.ChangeState(player.jumpState);
    }

    private void OnPlayerSlide()
    {
        player.ChangeState(player.slideState);
    }

    private void OnMobilePause()
    {
        if (player == null) return;
        pauseUi.SetActive(true);
        gameManager.ClickPause();
        player.PausePlayer();
    }

    private void OffMobilePause()
    {
        if (player == null) return;
        pauseUi.SetActive(false);
        gameManager.ClickPause();
        player.PausePlayer();
    }


    public void OnPauseUi() => pauseUi.SetActive(true);
    public void OffPauseUi() => pauseUi.SetActive(false);
}
