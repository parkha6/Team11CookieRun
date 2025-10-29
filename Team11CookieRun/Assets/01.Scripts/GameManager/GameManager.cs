using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum GameStage
{
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
    Button pauseButton;
    [SerializeField]
    Button pauseStartButton;
    [SerializeField]
    Button restartButton;
    GameStage currentStage = GameStage.Unknown;

    private void Awake()//시작시점에 필요한 변수를 로드하게 만들었음.
    {
        UIManager.Instance.LoadKey();
        pauseButton.onClick.AddListener(OnClickGamePause);
        pauseStartButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(OnClickRestart);
    }
    private void Start()
    { StartGame(); }
    private void Update()
    {
        if (UIManager.Instance.CurrentHp <= 0)
        { GameOver(); }
    }
    void OnClickGamePause()
    {
        if (currentStage != GameStage.End)
        {
            currentStage = GameStage.Pause;
            SetState();
        }
    }
    void GameOver()
    {
        currentStage = GameStage.End;
        SetState();
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
    void StartGame()
    {
        if (PauseUi.activeInHierarchy)
        { PauseUi.SetActive(false); }
        if (EndUi.activeInHierarchy)
        { EndUi.SetActive(false); }
            Time.timeScale = 1.0f;
        currentStage = GameStage.Start;
    }
    void SetState()
    {
        switch (currentStage)
        {
            case GameStage.Start:
                break;
            case GameStage.Pause:
                StopTime();
                PauseUi.SetActive(true);
                break;
            case GameStage.End:
                StopTime();
                EndUi.SetActive(true);
                break;
            case GameStage.Unknown:
            default:
                break;
        }
    }
}
