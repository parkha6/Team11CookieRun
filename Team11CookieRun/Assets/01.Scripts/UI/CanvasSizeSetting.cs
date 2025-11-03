using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 캔버스 화면비를 조절하기 위한 클래스
/// </summary>
public class CanvasSizeSetting : MonoBehaviour
{
    /// <summary>
    /// 어느 스크린인지 체크하는 값. Unknown으로 체크하면 maxCanvasRatio를 기준으로 expand로만 바꿔줌.
    /// </summary>
    [Tooltip("어느 스크린인지 체크하는 값. Unknown으로 체크하면 maxCanvasRatio를 기준으로 expand로만 바꿔줌.")]
    [SerializeField]
    WhichScreen whichScreen = WhichScreen.Unknown;
    /// <summary>
    /// 출력방식을 바꿀 UI창
    /// </summary>
    [Tooltip("출력방식을 바꿀 UI창")]
    [SerializeField] GameObject targetUI;
    /// <summary>
    /// 출력방식이 바뀔 화면비
    /// </summary>
    [Tooltip("출력방식이 바뀔 화면비")]
    [SerializeField]
    float maxCanvasRatio;
    /// <summary>
    /// 화면비를 조절하기 위해 받아오는 값
    /// </summary>
    CanvasScaler canvasScaler;
    /// <summary>
    /// 기본 화면비
    /// </summary>
    Vector2 defaultRatioVector2 = new Vector2(1920f, 1080f);
    /// <summary>
    /// 메인 매뉴의 최대 화면비
    /// </summary>
    Vector2 mainMenuMaxVector2 = new Vector2(641f, 752f);
    /// <summary>
    /// 게임화면의 최대 가로 화면비
    /// </summary>
    Vector2 gameSceneMaxWideVector2 = new Vector2(1210f, 444f);
    /// <summary>
    /// 게임화면의 최대 세로 화면비
    /// </summary>
    Vector2 gameSceneMaxHeightVector2 = new Vector2(900f, 647f);
    /// <summary>
    /// 클래스가 생성되면 전체 화면비를 보고 매치모드를 결정한다.
    /// </summary>
    private void Awake()
    {
        float currentScreenRatio = (float)Screen.width / Screen.height;
        if (targetUI == null)
        { Debug.Log("TargetUI를 배치하지 않았습니다."); }
        canvasScaler = targetUI.GetComponent<CanvasScaler>();
        if (canvasScaler == null)
        { Debug.Log("TargetUI에 CanvasScaler가 없습니다."); }

        switch (whichScreen)
        {
            case WhichScreen.MainMenuScene:
                if (currentScreenRatio < GmConst.mainMenuMaxRatio)
                {
                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    canvasScaler.matchWidthOrHeight = GmConst.minRatio;
                    canvasScaler.referenceResolution = mainMenuMaxVector2;
                }
                else
                {
                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    canvasScaler.matchWidthOrHeight = GmConst.mainMenuMatchRatio;
                    canvasScaler.referenceResolution = defaultRatioVector2;
                }
                break;
            case WhichScreen.GameScene:
                if (currentScreenRatio > GmConst.gameSceneMaxWideRatio)
                {
                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    canvasScaler.matchWidthOrHeight = GmConst.maxRatio;
                    canvasScaler.referenceResolution = gameSceneMaxWideVector2;
                }
                else if (currentScreenRatio < GmConst.gameSceneMaxHeightRatio)
                {
                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    canvasScaler.matchWidthOrHeight = GmConst.minRatio;
                    canvasScaler.referenceResolution = gameSceneMaxHeightVector2;

                }
                else
                {
                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    canvasScaler.matchWidthOrHeight = GmConst.gameSceneMatchRatio;
                    canvasScaler.referenceResolution = defaultRatioVector2;
                }
                break;
            case WhichScreen.Unknown:
            default:
                if (currentScreenRatio < maxCanvasRatio)
                { canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand; }
                break;
        }
    }
}