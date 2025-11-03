using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 메인화면 씬에서 재생되는 클래스. 메인화면 씬이 끝나면 사라짐.
/// </summary>
public class MenuUIManager : MonoBehaviour
{
    /// <summary>
    /// 게임 매니저를 할당하는 변수.
    /// </summary>
    GameManager gameManager;
    /// <summary>
    /// 게임매니저와 스코어 매니저를 변수에 넣고 버튼을 구독함
    /// </summary>
    private void Start()
    {
        gameManager = GameManager.Instance;
        if (Time.timeScale <= GmConst.stopTime)
        { gameManager.ManageTime(GmConst.runTime); }
        OnClickAddListeners();
    }
    /// <summary>
    /// 시작버튼을 누르면 이동하는 씬의 이름
    /// </summary>
    [Tooltip("시작버튼을 누르면 이동하는 씬의 이름")]
    [SerializeField] string gameSceneName;
    /// <summary>
    /// 시작버튼
    /// </summary>
    [Tooltip("시작버튼")]
    [SerializeField] internal Button startButton;
    /// <summary>
    /// 종료버튼
    /// </summary>
    [Tooltip("종료버튼")]
    [SerializeField] internal Button quitButton;
    /// <summary>
    /// 초기화 버튼
    /// </summary>
    [Tooltip("초기화 버튼")]
    [SerializeField] internal Button deleteDataButton;
    /// <summary>
    /// 메인화면에서 필요한 버튼을 구독하는 메서드
    /// </summary>
    void OnClickAddListeners()
    {
        if (startButton != null)
        { startButton.onClick.AddListener(StartGame); }

        if (quitButton != null)
        { quitButton.onClick.AddListener(gameManager.QuitGame); }
    }
    /// <summary>
    /// 스크립트가 파괴되면 버튼 구독해제
    /// </summary>
    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(StartGame);
        quitButton.onClick.RemoveListener(gameManager.QuitGame);
    }
    /// <summary>
    /// 시작 버튼을 누를때 작동하는 씬 이동
    /// </summary>
    void StartGame()
    { SceneManager.LoadScene(gameSceneName); }
}
