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
    /// <summary>
    /// 게임이 맨 처음 재생될때 필요한 클래스들을 가져오고 PlayerPrefs를 로드함.
    /// </summary>
    protected override void Awake()
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
    /// Start는 각 씬의 UI매니저에 있습니다.
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
                ManageTime(GmConst.stopTime);
                gameUIManager.ShowPauseUI();
                break;
            case GameStage.End:
                ManageTime(GmConst.stopTime);
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
    /// <summary>
    /// 게임이 시작될때 일시정지나 게임 완료 같은 UI를 한번 숨기고 시간을 재생한다.
    /// </summary>
    internal void StartGame()
    {
        currentStage = GameStage.Start;
        gameUIManager.HideUi();
        ManageTime(GmConst.runTime);
    }
    #endregion
    #region PauseGame
    /// <summary>
    /// 게임이 끝나거나 일시정지상태가 아니면 일시정지를 켜고 
    /// 일시정지면 일시정지를 끈다.
    /// </summary>
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
    /// <summary>
    /// pause상태에서 나와서 start로 돌아간다.
    /// </summary>
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
    /// <summary>
    /// HP를 초기화하고 IsDie bool을 풀고 현재 점수를 0으로 만드는 변수.
    /// 근데 제가 임의로 이러니까 조작키가 안 먹어서 유찬님이 만들어주시는게 좋을듯.
    /// </summary>
    internal void ResetValue()
    {
        if (player.Hp <= GmConst.dead)
        { player.Hp = player.MaxHp; }
        if (player.IsDie)
        { player.IsDie = false; }
        if (player.Score > 0)
        { player.Score = GmConst.minScore; }
    }
    /// <summary>
    /// whichScene에 입력된 string값이랑 같은 제목의 씬으로 이동하는 함수.
    /// </summary>
    /// <param name="whichScene"></param>
    internal void MoveScene(string whichScene)
    { SceneManager.LoadScene(whichScene); }
    /// <summary>
    /// inputTime의 값에 맞춰서 시간을 조작한다.
    /// </summary>
    /// <param name="inputTime"></param>
    internal void ManageTime(float inputTime)
    {
        if (inputTime < 0)
        { inputTime = 0; }
        Time.timeScale = inputTime;
    }
    /// <summary>
    /// 메모리에 있는 PlayerPrefs의 값을 하드에 저장한다.
    /// </summary>
    internal void SaveGame()//게임 저장
    { PlayerPrefs.Save(); }
    /// <summary>
    /// PlayerPrefs의 값을 모두 제거한다.
    /// </summary>
    internal void DeleteData()
    { PlayerPrefs.DeleteAll(); }
    /// <summary>
    /// 에디터면 에디터를 종료하고 본게임이면 본게임을 종료한다.
    /// </summary>
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
