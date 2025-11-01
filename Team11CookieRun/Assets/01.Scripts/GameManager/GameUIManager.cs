using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameUIManager : MonoBehaviour
{
    GameManager gameManager;
    ScoreManager scoreManager;
    /// <summary>
    /// 매니저 인스턴스들을 등록하고 게임매니저에 자기 자신을 집어넣은 뒤 버튼을 구독하고 스타트 게임으로 변수를 바꿈.
    /// 게임을 재시작했을때 여기서 세팅함.
    /// </summary>
    private void Start()
    {
        gameManager = GameManager.Instance;
        scoreManager = ScoreManager.Instance;
        gameManager.AddStartScene(this);
        OnClickAddListeners();
        gameManager.ResetValue();//TODO:임의로 이렇게 처리했는데 이러니까 재시작 했을때 키가 하나도 안 먹어요. 이건 hp랑 점수 리셋하는 메서드를 만들어서 건내주시면 금방 수정될 듯.
        gameManager.StartGame();
    }
    #region debugUI
    [Tooltip("데이터를 모두 지우는 버튼을 가진 UI(나중에 지울 예정)")]
    [SerializeField]
    internal GameObject debugUI;
    [Tooltip("데이터를 모두 지우는 버튼(나중에 지울 예정)")]
    [SerializeField]
    internal Button deleteDataButton;
    #endregion
    #region DefaultUI
    [Tooltip("재도전 버튼을 누르면 이동하는 게임 재생씬의 이름")]
    [SerializeField]
    internal string gameSceneName;
    [Tooltip("홈 버튼을 누르면 가는 메뉴 씬의 이름")]
    [SerializeField]
    internal string menuSceneName;
    [Tooltip("게임 상단에 표시되는 체력 바")]
    [SerializeField]
    internal Image hpBar;
    [Tooltip("게임 상단에 표시되는 점수 바")]
    [SerializeField]
    internal TextMeshProUGUI scoreText;
    #endregion
    #region PauseUI
    [Tooltip("화면 우측상단의 옵션 버튼을 누르면 나오는 일시정지 UI")]
    [SerializeField]
    internal GameObject pauseUi;
    [Tooltip("화면 우측상단에 표시 될 옵션버튼")]
    [SerializeField]
    internal Button pauseOptionButton;
    [Tooltip("일시정지 매뉴에 나오는 홈 버튼")]
    [SerializeField]
    internal Button pauseHomeButton;
    [Tooltip("일시정지 매뉴에 나오는 세팅 버튼")]
    [SerializeField]
    internal Button pauseSettingButton;
    [Tooltip("일시정지 매뉴에 나오는 Back 버튼")]
    [SerializeField]
    internal Button pauseBackButton;
    #endregion
    #region EndUI
    [Tooltip("결과창 UI")]
    [SerializeField]
    internal GameObject endUi;
    [Tooltip("결과창에 표시되는 점수 텍스트")]
    [SerializeField]
    internal TextMeshProUGUI finalScoreText;
    [Tooltip("결과창에 표시되는 최고 점수 텍스트")]
    [SerializeField]
    internal TextMeshProUGUI highscoreText;
    [Tooltip("최고 점수를 갱신하면 나오는 별 이미지")]
    [SerializeField]
    internal GameObject star;
    [Tooltip("최고 점수를 갱신하면 최고점수 옆에 나오는 New 버튼")]
    [SerializeField]
    internal GameObject newText;
    [Tooltip("결과창에 나오는 홈 버튼")]
    [SerializeField]
    internal Button endHomeButton;
    [Tooltip("결과창에 나오는 재시작 버튼")]
    [SerializeField]
    internal Button endRetryButton;//
    #endregion
    #region Mobile
    [SerializeField] Player player;
    //[SerializeField] private Button pauseOptionButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button slideButton;
    #endregion
    /// <summary>
    /// 버튼을 구독하는 메서드.
    /// </summary>
    void OnClickAddListeners()
    {
        if (pauseOptionButton != null)
        { pauseOptionButton.onClick.AddListener(OnMobilePause); }
        if (pauseHomeButton != null)
        { pauseHomeButton.onClick.AddListener(OnClickHome); }
        if (pauseBackButton != null)
        { pauseBackButton.onClick.AddListener(gameManager.OnClickExitPause); }
        if (endHomeButton != null)
        { endHomeButton.onClick.AddListener(OnClickHome); }
        if (endRetryButton != null)
        { endRetryButton.onClick.AddListener(Retry); }
        if (deleteDataButton != null)
        {
            deleteDataButton.onClick.AddListener(gameManager.DeleteData);
            if (gameManager.debugMode)
            { debugUI.SetActive(true); }
        }
        if (jumpButton != null) { jumpButton.onClick.AddListener(OnPlayerJump); }
        if (slideButton != null) { jumpButton.onClick.AddListener(OnPlayerSlide); }//TODO:슬라이드 버튼이 아니라 점프 버튼에 넣어요?
    }
    /// <summary>
    /// 스크립트가 파괴되면 버튼 구독을 취소함
    /// </summary>
    private void OnDestroy()
    {
        pauseOptionButton.onClick.RemoveListener(OnMobilePause);
        pauseHomeButton.onClick.RemoveListener(OnClickHome);
        pauseBackButton.onClick.RemoveListener(gameManager.OnClickExitPause);
        endHomeButton.onClick.RemoveListener(OnClickHome);
        endRetryButton.onClick.AddListener(Retry);
        deleteDataButton.onClick.RemoveListener(gameManager.DeleteData);
        jumpButton.onClick.RemoveListener(OnPlayerJump);
        jumpButton.onClick.RemoveListener(OnPlayerSlide);
    }
    /// <summary>
    /// 재시작 버튼을 누르면 작동하는 현재 씬을 다시 부르는 메서드
    /// </summary>
    void Retry()
    { gameManager.OnClickRetry(gameSceneName); }
    /// <summary>
    /// 점수 텍스트에 score를 띄움. 
    /// </summary>
    /// <param name="score"></param>
    internal void ShowScore(float score)
    { scoreText.text = score.ToString(); }
    /// <summary>
    /// 결과창에 현재점수를 띄우고 PlayerPrefs에서 최대점수를 불러와서 현재점수와 비교한 뒤 
    /// 현재점수가 더 크면 저장하고 별과 new를 띄운다.
    /// </summary>
    /// <param name="score"></param>
    internal void CompareScore(float score)
    {
        finalScoreText.text = score.ToString();
        if (score > scoreManager.HighScore || !PlayerPrefs.HasKey(GmConst.highScoreKey))
        {
            PlayerPrefs.SetFloat(GmConst.highScoreKey, score);
            star.SetActive(true);
            newText.SetActive(true);
        }
        scoreManager.HighScore = PlayerPrefs.GetFloat(GmConst.highScoreKey, GmConst.minScore);
        highscoreText.text = scoreManager.HighScore.ToString();
    }
    /// <summary>
    /// 현재 Hp값과 최대 Hp값을 넣으면 상단의 체력바에서 표시된다.
    /// </summary>
    /// <param name="currentHp"></param>
    /// <param name="hp"></param>
    internal void ShowHp(float currentHp, float hp)
    { hpBar.fillAmount = currentHp / hp; }
    /// <summary>
    /// 결과창의 별과 new표시를 감추는 메서드.
    /// </summary>
    internal void HideStar()
    {
        if (star.activeInHierarchy)
        { star.SetActive(false); }
        if (newText.activeInHierarchy)
        { newText.SetActive(false); }
    }
    /// <summary>
    /// 일시정지 UI를 보여주는 메서드
    /// </summary>
    internal void ShowPauseUI()
    { pauseUi.SetActive(true); }
    /// <summary>
    /// 결과 UI를 보여주는 메서드.
    /// </summary>
    internal void ShowEndUI()
    { endUi.SetActive(true); }
    /// <summary>
    /// 별,newtext,일시정지UI,결과UI를 모두 숨긴다.
    /// </summary>
    internal void HideUi()
    {
        HideStar();
        if (pauseUi.activeInHierarchy)
        { pauseUi.SetActive(false); }
        if (endUi.activeInHierarchy)
        { endUi.SetActive(false); }
    }
    /// <summary>
    /// 게임이 끝났을때 home버튼을 누르면 게임을 저장하고 UI를 숨긴뒤 
    /// 대기상태로 세팅하고 매뉴로 간다.
    /// </summary>
    internal void OnClickHome()
    {
        if (gameManager.currentStage == GameStage.End || gameManager.currentStage == GameStage.Pause)
        {
            gameManager.SaveGame();
            HideUi();
            gameManager.currentStage = GameStage.Waiting;
            gameManager.MoveScene(menuSceneName);
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
        gameManager.currentStage = GameStage.Pause; //TODO:일시정지 매뉴가 안 떠서 임시로 넣어놨어요.
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
