using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuUIManager : MonoBehaviour
{
    GameManager gameManager;
    ScoreManager scoreManager;
    /// <summary>
    /// 게임매니저와 스코어 매니저를 변수에 넣고 버튼을 구독함
    /// </summary>
    private void Start()
    {
        gameManager = GameManager.Instance;
        scoreManager = ScoreManager.Instance;
        OnClickAddListeners();
    }
    [Tooltip("시작버튼을 누르면 이동하는 씬의 이름")]
    [SerializeField]
    string gameSceneName;
    [Tooltip("시작버튼")]
    [SerializeField]
    internal Button startButton;
    [Tooltip("종료버튼")]
    [SerializeField]
    internal Button quitButton;
    [Tooltip("초기화 버튼")]
    [SerializeField]
    internal Button deleteDataButton;
    /// <summary>
    /// 시작버튼과 종료버튼 구독시작
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
