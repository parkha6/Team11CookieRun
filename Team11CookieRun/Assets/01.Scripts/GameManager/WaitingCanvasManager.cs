using UnityEngine;
using UnityEngine.UI;

public class WaitingCanvasManager : SingletonManager<WaitingCanvasManager>
{
    [SerializeField]
    internal string homeSceneName;//홈씬의 이름
    [SerializeField]
    internal Button startButton;
    [SerializeField]
    internal Button deleteDataButton;
    [SerializeField]
    internal Button quitButton;
}
