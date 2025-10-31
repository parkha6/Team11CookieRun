using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum GameStage
{
    Waiting,
    Start,
    Pause,
    End,
    Unknown
}
public class GameManager : SingletonManager<GameManager>
{
    [SerializeField]
    string homeSceneName;//홈씬의 이름
    [SerializeField]
    string sceneName;//게임씬의 이름.
    [SerializeField]//디버그용으로 시작스테이지 쓸려고 공개해둠.
    GameStage currentStage = GameStage.Unknown;
    #region Button Input Field
    [SerializeField]
    Button startButton;
    [SerializeField]
    Button pauseOptionButton;
    [SerializeField]
    Button pauseHomeButton;
    [SerializeField]
    Button pauseSettingButton;
    [SerializeField]
    Button pauseExitButton;
    [SerializeField]
    Button homeButton;
    [SerializeField]
    Button restartButton;
    [SerializeField]
    Button deleteDataButton;
    [SerializeField]
    Button quitButton;
    #endregion
    #region Other Manager
    UIManager uiManager;//UI매니저 받아오기용
    #endregion
    #region YouChan
    //Start부분
    private bool isStart = false;
    //일시정지
    private bool isPause = false;
    //정지
    public bool IsStart { get { return isStart; } set { isStart = value; } }
    public bool IsPause { get { return isPause; } set { isPause = value; } }
    #endregion
    #region Life Cycle
    protected override void Awake()//시작시점에 필요한 변수를 로드하게 만들었음.
    {
        uiManager = UIManager.Instance;
        uiManager.LoadKey();
        AddOnClickButton();
    }
    private void Start()//씬이 바뀔거 같은 부분만 살려놨음.
    {
        switch (currentStage)
        {
            case GameStage.Waiting:
                WaitGame();
                break;
            case GameStage.Start:
                StartGame();
                break;
            case GameStage.Pause:
            case GameStage.End:
            case GameStage.Unknown:
            default:
                break;
        }
    }
    private void Update()
    {
        switch (currentStage)
        {
            case GameStage.Waiting:
                break;
            case GameStage.Start:
                uiManager.ShowHp();
                uiManager.ShowScore();
                if (uiManager.CurrentHp <= 0)
                {currentStage = GameStage.End;}
                break;
            case GameStage.Pause:
                    StopTime();
                    uiManager.ShowPauseUI();
                break;
            case GameStage.End:
                    StopTime();
                    uiManager.CompareScore();
                    uiManager.ShowEndUI();
                break;
            case GameStage.Unknown:
            default:
                break;
        }
    }
    #endregion
    #region Awake Setting
    void AddOnClickButton()//버튼과 함수 연결
    {
        if (startButton != null)
        { startButton.onClick.AddListener(StartGame); }
        if (pauseOptionButton != null)
        { pauseOptionButton.onClick.AddListener(OnClickGamePause); }
        if (pauseHomeButton != null)
        { pauseHomeButton.onClick.AddListener(OnClickHome); }
        if (pauseExitButton != null)
        { pauseExitButton.onClick.AddListener(OnClickExitPause); }
        if (homeButton != null)
        { homeButton.onClick.AddListener(OnClickHome); }
        if (restartButton != null)
        { restartButton.onClick.AddListener(OnClickRestart); }
        if (deleteDataButton != null)
        { deleteDataButton.onClick.AddListener(DeleteData); }
        if (quitButton != null)
        { quitButton.onClick.AddListener(QuitGame); }
    }
    #endregion
    #region Waiting
    void WaitGame()
    { uiManager.HideUi(); }
    #endregion
    #region Starting
    void StartGame()
    {
        uiManager.HideUi();
        currentStage = GameStage.Start;
        RunTime();
    }
    #endregion
    #region PauseGame
    void OnClickGamePause()
    {
        if (currentStage != GameStage.End && currentStage != GameStage.Pause)
        {
            currentStage = GameStage.Pause;
            IsPause = true;
        }
        else if (currentStage == GameStage.Pause)
        { OnClickExitPause(); }
    }
    void OnClickExitPause()
    {
        IsPause = false;
        StartGame();
    }
    #endregion
    #region EndGame
    void OnClickHome()
    {
        if (currentStage == GameStage.End)
        {
            //isEnd = false;
            SaveGame();
            uiManager.ResetScore();
            MoveScene(homeSceneName);
        }
    }
    void OnClickRestart()
    {
        if (currentStage == GameStage.End)
        {
            //isEnd = false;
            SaveGame();
            uiManager.ResetScore();
            MoveScene(sceneName);
        }
    }
    #endregion
    #region Utility
    void MoveScene(string whichScene)
    { SceneManager.LoadScene(whichScene); }
    void StopTime()//시간을 멈추는 함수
    { Time.timeScale = 0; }
    void RunTime()//시간을 재생하는 함수
    { Time.timeScale = 1.0f; }
    void SaveGame()//게임 저장
    { PlayerPrefs.Save(); }
    void DeleteData()
    { PlayerPrefs.DeleteAll(); }
    void QuitGame()//게임 종료 함수
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    #endregion

    #region YouChan
    private void PauseGame()
    {
        if (IsPause) return;

        IsPause = true;
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        if (!IsPause) return;

        IsPause = false;
        Time.timeScale = 1f;
    }

    public void ClickPause()
    {
        if (IsPause)
            ResumeGame();
        else
            PauseGame();
    }
    #endregion
}
