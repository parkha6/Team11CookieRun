using UnityEngine;

/// <summary>
/// 씬이 어떤 상태인지 표시하는 enum
/// </summary>
public enum GameStage
{
    Waiting,
    Start,
    Pause,
    End,
    Unknown
}
/// <summary>
/// 화면비를 인스펙터에서 쉽게 조정하기 위해 세팅해놓은 enum값
/// </summary>
internal enum WhichScreen
{
    MainMenuScene,
    GameScene,
    Unknown,
}