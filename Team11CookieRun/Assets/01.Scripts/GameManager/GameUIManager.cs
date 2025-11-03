using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 게임씬에서 재생되는 클래스. 게임씬이 넘어가면 사라짐.
/// </summary>
public class GameUIManager : MonoBehaviour
{
    /// <summary>
    /// 게임매니저 넣는 변수
    /// </summary>
    GameManager gameManager;
    /// <summary>
    /// 스코어 매니저 넣는 변수
    /// </summary>
    ScoreManager scoreManager;
    /// <summary>
    /// 이미지 색을 바꾸기 위한 변수 녹색 : #356F34 노랑 :#E38F08 빨강:#BF0911
    /// 더 스텍 참고해서 색이 서서히 변하게 해야지.
    /// </summary>
    Image hpBarImage;
    /// <summary>
    /// 풀피일때 나오는 초록색
    /// </summary>
    Color green = new Color(53f / 255f, 111f / 255f, 52f / 255f, 1f);
    /// <summary>
    /// 중간피일때 나오는 노란색
    /// </summary>
    Color yellow = new Color(227f / 255f, 143f / 255f, 8f / 255f, 1f);
    /// <summary>
    /// 피가 거의 없을때 나오는 빨간색
    /// </summary>
    Color red = new Color(190f / 255f, 8f / 255f, 16f / 255, 1f);
    #region debugUI
    /// <summary>
    /// 데이터를 모두 지우는 버튼을 가진 UI(나중에 지울 예정)
    /// </summary>
    [Tooltip("데이터를 모두 지우는 버튼을 가진 UI(나중에 지울 예정)")]
    [SerializeField] internal GameObject debugUI;
    /// <summary>
    /// 데이터를 모두 지우는 버튼(나중에 지울 예정)
    /// </summary>
    [Tooltip("데이터를 모두 지우는 버튼(나중에 지울 예정)")]
    [SerializeField] internal Button deleteDataButton;
    #endregion
    /// <summary>
    /// 재도전 버튼을 누르면 이동하는 게임 재생씬의 이름
    /// </summary>
    #region DefaultUI
    [Tooltip("재도전 버튼을 누르면 이동하는 게임 재생씬의 이름")]
    [SerializeField] internal string gameSceneName;
    /// <summary>
    /// 홈 버튼을 누르면 가는 메뉴 씬의 이름
    /// </summary>
    [Tooltip("홈 버튼을 누르면 가는 메뉴 씬의 이름")]
    [SerializeField] internal string menuSceneName;
    /// <summary>
    /// 게임 상단에 표시되는 체력 바
    /// </summary>
    [Tooltip("게임 상단에 표시되는 체력 바")]
    [SerializeField] internal Image hpBar;
    /// <summary>
    /// 게임 상단에 표시되는 점수 바
    /// </summary>
    [Tooltip("게임 상단에 표시되는 점수 바")]
    [SerializeField] internal TextMeshProUGUI scoreText;
    #endregion
    #region PauseUI
    /// <summary>
    /// 화면 우측상단의 옵션 버튼을 누르면 나오는 일시정지 UI
    /// </summary>
    [Tooltip("화면 우측상단의 옵션 버튼을 누르면 나오는 일시정지 UI")]
    [SerializeField] internal GameObject pauseUi;
    /// <summary>
    /// 화면 우측상단에 표시 될 옵션버튼
    /// </summary>
    [Tooltip("화면 우측상단에 표시 될 옵션버튼")]
    [SerializeField] internal Button pauseOptionButton;
    /// <summary>
    /// 일시정지 매뉴에 나오는 홈 버튼
    /// </summary>
    [Tooltip("일시정지 매뉴에 나오는 홈 버튼")]
    [SerializeField] internal Button pauseHomeButton;
    /// <summary>
    /// 일시정지 매뉴에 나오는 세팅 버튼
    /// </summary>
    [Tooltip("일시정지 매뉴에 나오는 세팅 버튼")]
    [SerializeField] internal Button pauseSettingButton;
    /// <summary>
    /// 일시정지 매뉴에 나오는 Back 버튼
    /// </summary>
    [Tooltip("일시정지 매뉴에 나오는 Back 버튼")]
    [SerializeField] internal Button pauseBackButton;
    #endregion
    #region EndUI
    /// <summary>
    /// 결과창 UI
    /// </summary>
    [Tooltip("결과창 UI")]
    [SerializeField] internal GameObject endUi;
    /// <summary>
    /// 결과창에 표시되는 점수 텍스트
    /// </summary>
    [Tooltip("결과창에 표시되는 점수 텍스트")]
    [SerializeField] internal TextMeshProUGUI finalScoreText;
    /// <summary>
    /// 결과창에 표시되는 최고 점수 텍스트
    /// </summary>
    [Tooltip("결과창에 표시되는 최고 점수 텍스트")]
    [SerializeField] internal TextMeshProUGUI highscoreText;
    /// <summary>
    /// 최고 점수를 갱신하면 나오는 별 이미지
    /// </summary>
    [Tooltip("최고 점수를 갱신하면 나오는 별 이미지")]
    [SerializeField] internal GameObject star;
    /// <summary>
    /// 최고 점수를 갱신하면 최고점수 옆에 나오는 New 버튼
    /// </summary>
    [Tooltip("최고 점수를 갱신하면 최고점수 옆에 나오는 New 버튼")]
    [SerializeField]internal GameObject newText;
    /// <summary>
    /// 결과창에 나오는 홈 버튼
    /// </summary>
    [Tooltip("결과창에 나오는 홈 버튼")]
    [SerializeField]internal Button endHomeButton;
    /// <summary>
    /// 결과창에 나오는 재시작 버튼
    /// </summary>
    [Tooltip("결과창에 나오는 재시작 버튼")]
    [SerializeField]internal Button endRetryButton;
    #endregion
    #region Mobile
    public Player player;
    [SerializeField] private GameObject mobileObject;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button slideButton;
    #endregion
    #region Item Buff
    [SerializeField] private Transform buffTransform;
    #endregion
    /// <summary>
    /// 매니저 인스턴스들을 등록하고 게임매니저에 자기 자신을 집어넣은 뒤 버튼을 구독하고 스타트 게임으로 변수를 바꿈.
    /// 게임을 재시작했을때 여기서 세팅함.
    /// </summary>
    private void Start()
    {
        gameManager = GameManager.Instance;
        scoreManager = ScoreManager.Instance;
        hpBarImage = hpBar.GetComponent<Image>();
        gameManager.ManageTime(GmConst.runTime);
        gameManager.AddStartScene(this);
        OnClickAddListeners();
        gameManager.StartGame();
#if UNITY_ANDROID || UNITY_IOS
        mobileObject.SetActive(true);
#endif
    }
    /// <summary>
    /// 버튼을 구독하는 메서드.
    /// </summary>
    void OnClickAddListeners()
    {
        if (pauseOptionButton != null)
        { pauseOptionButton.onClick.AddListener(OnMobilePause); }
        if (pauseHomeButton != null)
        { pauseHomeButton.onClick.AddListener(OnClickHome); }
#if UNITY_ANDROID || UNITY_IOS
        if (pauseBackButton != null)
        { pauseBackButton.onClick.AddListener(OffMobilePause); }
#else
        if (pauseBackButton != null)
        { pauseBackButton.onClick.AddListener(gameManager.OnClickExitPause); }
#endif
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
    }
    /// <summary>
    /// 스크립트가 파괴되면 버튼 구독을 취소함
    /// </summary>
    private void OnDestroy()
    {
        pauseOptionButton.onClick.RemoveListener(OnMobilePause);
        pauseHomeButton.onClick.RemoveListener(OnClickHome);
#if UNITY_ANDROID || UNITY_IOS
        { pauseBackButton.onClick.RemoveListener(OffMobilePause); }
#else
        pauseBackButton.onClick.RemoveListener(gameManager.OnClickExitPause);
#endif
        endHomeButton.onClick.RemoveListener(OnClickHome);
        endRetryButton.onClick.AddListener(Retry);
        deleteDataButton.onClick.RemoveListener(gameManager.DeleteData);
        jumpButton.onClick.RemoveListener(OnPlayerJump);
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
    {
        float hpRatio = currentHp / hp;
        hpBar.fillAmount = hpRatio;
        float normalizedRatio;

        if (hpRatio >= GmConst.halfHp)
        {
            normalizedRatio = (hpRatio - GmConst.halfHp) * 2;
            hpBarImage.color = MakeHpColor(green, yellow, normalizedRatio);
        }
        else if (hpRatio < GmConst.halfHp)
        {
            normalizedRatio = hpRatio * 2;
            hpBarImage.color = MakeHpColor(yellow, red, normalizedRatio);
        }
    }
    /// <summary>
    /// startColor와 EndColor사이의 PercentValue의 값을 리턴한다. HP바용으로 만들었음.
    /// </summary>
    /// <param name="startColor"></param>
    /// <param name="EndColor"></param>
    /// <param name="percentValue"></param>
    /// <returns></returns>
    Color MakeHpColor(Color startColor, Color endColor, float percentValue)
    {
        Color nextColor = Color.Lerp(endColor, startColor, percentValue);
        return nextColor;
    }
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
            gameManager.currentStage = gameManager.SetGameStage(GameStage.Waiting);
            gameManager.MoveScene(menuSceneName);
        }
    }

    private void OnPlayerJump()
    {
        if (player == null) return;
        if (gameManager.IsStart && player.IsSlide == false && player.IsJump == false)
        {
            player.IsJump = true;
            player.ChangeState(player.jumpState);
        }
    }

    public void OnPlayerSlide()
    {
        if (player == null) return;
        if (gameManager.IsStart && player.IsSlide == false && player.IsJump == false)
        {
            player.IsSlide = true;
            player.ChangeState(player.slideState);
        }
    }

    public void OffPlayerSlide()
    {
        if (player == null) return;
        if (gameManager.IsStart)
        {
            player.IsSlide = false;
            player.IsRun = true;
            player.ChangeState(player.runState);
        }
    }


    private void OnMobilePause()
    {
        gameManager.currentStage = gameManager.SetGameStage(GameStage.Pause);
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

    public GameObject OnBuffUi(GameObject buff)
    {
        return Instantiate(buff, buffTransform);
    }
}
