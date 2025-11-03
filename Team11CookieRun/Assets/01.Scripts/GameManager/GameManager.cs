using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 게임의 재생단계를 관리하는 클래스. (예시 : 메인화면>게임시작>일시정지>게임결과...)
/// </summary>
public class GameManager : SingletonManager<GameManager>
{
    /// <summary>
    /// 디버그 모드를 켜는 불리언 값
    /// </summary>
    [Tooltip("디버그 모드를 켜는 불리언 값")]
    [SerializeField]
    internal bool debugMode = false;
    /// <summary>
    /// 게임의 전체적인 상태를 컨트롤하는 enum변수.
    /// </summary>
    internal GameStage currentStage = GameStage.Unknown;
    #region Other Manager
    /// <summary>
    /// 점수 매니저 받아오기 용.
    /// </summary>
    ScoreManager scoreManager;
    /// <summary>
    /// 플레이어 클래스 받아오기 용.
    /// </summary>
    public Player player;
    /// <summary>
    /// 게임 UI매니저 넣는 변수
    /// </summary>
    GameUIManager gameUIManager;
    /// <summary>
    /// 게임 BGM매니저 넣는 변수
    /// </summary>
    GameBgmManager gameBgmManager;
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
        scoreManager = ScoreManager.Instance; 
        scoreManager.LoadKey();
    }
    /// <summary>
    /// 게임 UI 매니저를 게임매니저에 넣기 위해 만든 함수.
    /// </summary>
    /// <param name="startScene"></param>
    internal void AddStartScene(GameUIManager startScene)
    { gameUIManager = startScene; }

    internal void AddGameBGM(GameBgmManager bgmManager)
    { gameBgmManager = bgmManager; }
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
                { currentStage = SetGameStage(GameStage.End); }
                break;
            case GameStage.Pause:
                break;
            case GameStage.End:
                break;
            case GameStage.Unknown:
            default:
                break;
        }
    }
    /// <summary>
    /// changeStage에 바꿀 스테이지를 넣으면 필수적인 세팅을 하고 스테이지를 반환한다. 
    /// </summary>
    /// <param name="changeStage"></param>
    /// <returns></returns>
    internal GameStage SetGameStage(GameStage changeStage)
    {
        switch (changeStage)
        {
            case GameStage.Waiting:
                return GameStage.Waiting;
            case GameStage.Start:
                BgmSetting(gameBgmManager.stageBgm, true);
                return GameStage.Start;
            case GameStage.Pause:
                ManageTime(GmConst.stopTime);
                gameUIManager.ShowPauseUI();
                return GameStage.Pause;
            case GameStage.End:
                BgmSetting(gameBgmManager.resultBgm, false);
                //ManageTime(GmConst.stopTime);
                gameUIManager.CompareScore(player.Score);
                gameUIManager.ShowEndUI();
                return GameStage.End;
            case GameStage.Unknown:
            default:
                return GameStage.Unknown;
        }
    }
    #endregion
    #region Starting
    /// <summary>
    /// 게임이 시작될때 일시정지나 게임 완료 같은 UI를 한번 숨기고 시간을 재생한다.
    /// </summary>
    internal void StartGame()
    {
        currentStage = SetGameStage(GameStage.Start);
        gameUIManager.HideUi();
        ManageTime(GmConst.runTime);
    }
    /// <summary>
    /// clip에 원하는 음악을 넣고 isLoop에 루프를 할지 말지 bool 값을 입력하면 배경음악을 재생해준다.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="isLoop"></param>
    void BgmSetting(AudioClip clip,bool isLoop)
    {
        gameBgmManager.audioSource.Stop();
        gameBgmManager.audioSource.clip = clip;
        gameBgmManager.audioSource.Play();
        gameBgmManager.audioSource.loop = isLoop;
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
            currentStage = SetGameStage(GameStage.Pause);
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
        player.PausePlayer();
        StartGame();
    }
    #endregion
    #region EndGame
    /// <summary>
    /// Retry키를 누르면 작동하는 메서드
    /// </summary>
    /// <param name="sceneName"></param>
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
    /// 재시작할때 bool값 다시 세팅.
    /// </summary>
    internal void ResetValue()
    {
        IsStart = false;
        IsPause = false;
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
    /// 메모리에 있는 데이터를 저장한 후 에디터면 에디터를 종료하고 본게임이면 본게임을 종료한다.
    /// </summary>
    internal void QuitGame()
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
        {
            currentStage = SetGameStage(GameStage.Start);
            ResumeGame();
        }
        else
        {
            currentStage = SetGameStage(GameStage.Pause);
            PauseGame();
        }
    }
    #endregion
}
