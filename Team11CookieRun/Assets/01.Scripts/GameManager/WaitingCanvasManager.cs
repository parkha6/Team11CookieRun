using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class WaitingCanvasManager : MonoBehaviour
{
    GameManager gameManager;
    UIManager uiManager;
    private void Awake()//시작시점에 필요한 변수를 로드하게 만들었음.
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        OnClickAddListeners();
    }
    [SerializeField]
    internal string homeSceneName;//홈씬의 이름
    [SerializeField]
    string startSceneName;
    [SerializeField]
    internal Button startButton;
    [SerializeField]
    internal Button deleteDataButton;
    [SerializeField]
    internal Button quitButton;
    void OnClickAddListeners()
    {
        if (startButton != null)
        { startButton.onClick.AddListener(StartGame); }

        if (quitButton != null)
        { quitButton.onClick.AddListener(gameManager.QuitGame); }
    }
    void StartGame()
    { SceneManager.LoadScene(startSceneName); }
    internal void OnClickHome()
    {
        if (gameManager.currentStage == GameStage.End)
        {
            //isEnd = false;
            gameManager.SaveGame();
            uiManager.ResetScore();
            gameManager.MoveScene(homeSceneName);
        }
    }
}
