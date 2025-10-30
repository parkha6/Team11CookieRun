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
    bool debugMode = false;
    [SerializeField]
    string sceneName;//재시작할 씬의 이름.
    [SerializeField]
    GameObject EndUi;
    [SerializeField]
    GameObject PauseUi;
    [SerializeField]
    Button startButton;
    [SerializeField]
    Button pauseOptionButton;
    [SerializeField]
    Button pauseExitButton;
    [SerializeField]
    Button restartButton;
    [SerializeField]
    Button quitButton;
    [SerializeField]//디버그용으로 시작스테이지 쓸려고 공개해둠.
    GameStage currentStage = GameStage.Unknown;
    #region YouChan
    //Start부분
    private bool isStart = false;
    //일시정지
    private bool isPause = false;
    //정지
    public bool IsStart { get { return isStart; } set { isStart = value; } }
    public bool IsPause { get { return isPause; } set { isPause = value; } }
    #endregion
    private bool isEnd = false;
    public bool IsEnd { get { return isEnd; } set { isEnd = value; } }
    protected override void Awake()//시작시점에 필요한 변수를 로드하게 만들었음.
    {
        
        UIManager.Instance.LoadKey();
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
                UIManager.Instance.ShowScore();
                if (debugMode)
                { UIManager.Instance.CurrentHp -= 100f * Time.deltaTime; }
                if (UIManager.Instance.CurrentHp <= 0)
                {
                    currentStage = GameStage.End;
                    isEnd = true;
                }
                break;
            case GameStage.Pause:
                if (IsPause)
                {
                    StopTime();
                    PauseUi.SetActive(true);
                }
                break;
            case GameStage.End:
                if (isEnd)
                {
                    StopTime();
                    UIManager.Instance.CompareScore();
                    EndUi.SetActive(true);
                }
                break;
            case GameStage.Unknown:
            default:
                break;
        }
    }
    void AddOnClickButton()
    {
        if (startButton != null)
        { startButton.onClick.AddListener(StartGame); }
        if (pauseOptionButton != null)
        { pauseOptionButton.onClick.AddListener(OnClickGamePause); }
        if (pauseExitButton != null)
        { pauseExitButton.onClick.AddListener(OnClickExitPause); }
        if (restartButton != null)
        { restartButton.onClick.AddListener(OnClickRestart); }
        if (quitButton != null)
        { quitButton.onClick.AddListener(QuitGame); }
    }
    void WaitGame()
    { HideUi(); }
    void StartGame()
    {
        HideUi();
        currentStage = GameStage.Start;
        
        RunTime();
    }
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
    void OnClickRestart()
    {
        if (currentStage == GameStage.End)
        {
            isEnd = false;
            SceneManager.LoadScene(sceneName);
        }
    }//씬을 완전히 새로 시작?

    void StopTime()//시간을 멈추는 함수
    { Time.timeScale = 0;}
    void RunTime()
    { Time.timeScale = 1.0f; }
    void HideUi()
    {
        if (PauseUi.activeInHierarchy)
        { PauseUi.SetActive(false); }
        if (EndUi.activeInHierarchy)
        { EndUi.SetActive(false); }
    }
    internal void SaveGame()
    { PlayerPrefs.Save(); }
    internal void QuitGame()//게임 종료 함수
    {
        UIManager.Instance.SaveGame();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
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
