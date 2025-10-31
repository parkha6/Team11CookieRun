using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StartCanvasManager : SingletonManager<StartCanvasManager>
{
    WaitingCanvasManager waitingCanvasManager;
    StartCanvasManager startCanvasManager;
    protected override void Awake()//시작시점에 필요한 변수를 로드하게 만들었음.
    {
        waitingCanvasManager = WaitingCanvasManager.Instance;
        startCanvasManager = StartCanvasManager.Instance;
    }
    #region debugUI
    [SerializeField]
    internal GameObject debugUI;
    [SerializeField]
    internal Button deleteDataButton;
    #endregion
    #region DefaultUI
    [SerializeField]
    internal string sceneName;
    [SerializeField]
    internal Image hpBar;
    [SerializeField]
    internal TextMeshProUGUI scoreText;
    #endregion
    #region PauseUI
    [SerializeField]
    internal GameObject PauseUi;
    [SerializeField]
    internal Button pauseOptionButton;
    [SerializeField]
    internal Button pauseHomeButton;
    [SerializeField]
    internal Button pauseSettingButton;
    [SerializeField]
    internal Button pauseExitButton;
    [SerializeField]
    internal Button homeButton;
    [SerializeField]
    internal Button restartButton;
    #endregion
    #region EndUI
    [SerializeField]
    internal GameObject EndUi;
    [SerializeField]
    internal TextMeshProUGUI finalScoreText;
    [SerializeField]
    internal TextMeshProUGUI highscoreText;
    [SerializeField]
    internal GameObject star;
    [SerializeField]
    internal GameObject newText;
    #endregion
}
