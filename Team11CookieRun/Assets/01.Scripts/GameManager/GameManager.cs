using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : SingletonManager<GameManager>
{
    [Tooltip("디버그 모드를 켜는 불리언 값")]
    [SerializeField]
    internal bool debugMode = false;
    [Tooltip("메인씬을 다 안 만들었을때 Waiting에서 넘어갈 방법이 없어서 공개해 놓은 값")]
    [SerializeField]
    internal GameStage currentStage = GameStage.Unknown;
    #region Other Manager
    ScoreManager uiManager;//UI매니저 받아오기용
    Player player;
    GameUIManager gameUIManager;
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
        player = FindObjectOfType<Player>();
        uiManager = ScoreManager.Instance;
        uiManager.LoadKey();
    }
    /// <summary>
    /// 게임 UI 매니저를 게임매니저에 넣기 위해 만든 함수.
    /// </summary>
    /// <param name="startScene"></param>
    internal void AddStartScene(GameUIManager startScene)
    { gameUIManager = startScene; }
    /// <summary>
    /// 스위치 루프를 돌면서 currentStage의 값에 따라 업데이트를 돌린다.
    /// </summary>
    private void Update()
    {
        switch (currentStage)
        {
            case GameStage.Waiting:
                break;
            case GameStage.Start:
                gameUIManager.ShowHp(player.Hp, player.MaxHp);
                gameUIManager.ShowScore(player.Score);
                if (player.IsDie)
                { currentStage = GameStage.End; }
                break;
            case GameStage.Pause:
                StopTime();
                gameUIManager.ShowPauseUI();
                break;
            case GameStage.End:
                StopTime();
                gameUIManager.CompareScore(player.Score);
                gameUIManager.ShowEndUI();
                break;
            case GameStage.Unknown:
            default:
                break;
        }
    }
    #endregion
    #region Starting
    internal void StartGame()//게임이 시작될때 세팅.
    {
        gameUIManager.HideUi();
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
    internal void OnClickRetry(string sceneName)
    {
        if (currentStage == GameStage.End)
        {
            SaveGame();
            ResetValue();
            MoveScene(sceneName);
        }
    }
    #endregion
    #region Utility
    internal void ResetValue()
    {
        if (player.Hp <= GmConst.dead )
        { player.Hp = player.MaxHp; }
        if (player.IsDie)
        { player.IsDie = false; }
        if (player.Score > 0)
        { player.Score = GmConst.minScore; }
    }
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
