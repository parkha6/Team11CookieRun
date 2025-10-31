using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : SingletonManager<GameManager>
{
    [SerializeField]
    internal bool debugMode = false;
    [SerializeField]
    internal GameStage currentStage = GameStage.Unknown;
    #region Other Manager
    UIManager uiManager;//UI매니저 받아오기용
    StartCanvasManager startCanvasManager;
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

    internal void AddStartScene(StartCanvasManager startScene)
    { startCanvasManager = startScene; }
    private void Update()
    {
        switch (currentStage)
        {
            case GameStage.Waiting:
                break;
            case GameStage.Start:
                startCanvasManager.ShowHp();
                startCanvasManager.ShowScore();
                if (uiManager.IsDead())
                {currentStage = GameStage.End;}
                break;
            case GameStage.Pause:
                    StopTime();
                startCanvasManager.ShowPauseUI();
                break;
            case GameStage.End:
                    StopTime();
                startCanvasManager.CompareScore();
                startCanvasManager.ShowEndUI();
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

    }
    #endregion
    #region Starting
    internal void StartGame()//게임이 시작될때 세팅.
    {
        startCanvasManager.HideUi();
        currentStage = GameStage.Start;
        RunTime();
    }
    #endregion
    #region PauseGame
    internal void OnClickGamePause()
    {
        if (currentStage != GameStage.End && currentStage != GameStage.Pause)
        {
            currentStage = GameStage.Pause;
            IsPause = true;
        }
        else if (currentStage == GameStage.Pause)
        { OnClickExitPause(); }
    }
    internal void OnClickExitPause()
    {
        IsPause = false;
        StartGame();
    }
    #endregion
    #region EndGame
    internal void OnClickRestart(string sceneName)
    {
        if (currentStage == GameStage.End)
        {

            SaveGame();
            uiManager.ResetScore();
            MoveScene(sceneName);
        }
    }
    #endregion
    #region Utility
    internal void MoveScene(string whichScene)
    { SceneManager.LoadScene(whichScene); }
    void StopTime()//시간을 멈추는 함수
    { Time.timeScale = 0; }
    void RunTime()//시간을 재생하는 함수
    { Time.timeScale = 1.0f; }
    internal void SaveGame()//게임 저장
    { PlayerPrefs.Save(); }
    internal void DeleteData()
    { PlayerPrefs.DeleteAll(); }
    internal void QuitGame()//게임 종료 함수
    {
        SaveGame();
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
