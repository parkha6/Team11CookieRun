using Unity.VisualScripting;
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
    string sceneName;//재시작할 씬의 이름.
    [SerializeField]
    GameObject EndUi;
    [SerializeField]
    GameObject PauseUi;
    [SerializeField]
    Button startButton;
    [SerializeField]
    Button pauseButton;
    [SerializeField]
    Button pauseStartButton;
    [SerializeField]
    Button restartButton;
    [SerializeField]
    Button quitButton;
    GameStage currentStage = GameStage.Unknown;

    private void Awake()//시작시점에 필요한 변수를 로드하게 만들었음.
    {
        UIManager.Instance.LoadKey();
        AddOnClickButton();
    }
    private void Start()
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
                StopTime();
                PauseUi.SetActive(true);
                break;
            case GameStage.End:
                StopTime();
                UIManager.Instance.CompareScore();
                EndUi.SetActive(true);
                break;
            case GameStage.Unknown:
            default:
                break;
        }
        StartGame();
    }
    private void Update()
    {
        switch (currentStage)
        {
            case GameStage.Waiting:
                break;
            case GameStage.Start:
                if (UIManager.Instance.CurrentHp <= 0)
                { currentStage = GameStage.End; }
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
    void AddOnClickButton()
    {
        startButton.onClick.AddListener(OnClickStart);
        pauseButton.onClick.AddListener(OnClickGamePause);
        pauseStartButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(OnClickRestart);
        quitButton.onClick.AddListener(QuitGame);
    }
    void WaitGame()
    {
        if (PauseUi.activeInHierarchy)
        { PauseUi.SetActive(false); }
        if (EndUi.activeInHierarchy)
        { EndUi.SetActive(false); }
    }
    void OnClickStart()
    { currentStage = GameStage.Start; }
    void StartGame()
    {
        if (PauseUi.activeInHierarchy)
        { PauseUi.SetActive(false); }
        if (EndUi.activeInHierarchy)
        { EndUi.SetActive(false); }
        Time.timeScale = 1.0f;
    }
    void OnClickGamePause()
    {
        if (currentStage != GameStage.End)
        { currentStage = GameStage.Pause; }
    }
    void OnClickRestart()
    {
        if (currentStage == GameStage.End)
        { SceneManager.LoadScene(sceneName); }
    }//씬을 완전히 새로 시작?

    void StopTime()
    {
        if (currentStage != GameStage.Pause)
        { Time.timeScale = 0; }
    }
    internal void SaveGame()
    { PlayerPrefs.Save(); }
    internal void QuitGame()//게임 종료 함수
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
