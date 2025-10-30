using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public void StartGame()
    {
        Debug.Log("게임 시작 버튼이 클릭되었습니다.");
        // "SampleScene"이라는 이름의 씬으로 이동합니다.
        SceneManager.LoadScene("SampleScene");
    }

    private bool CanEnterMiniGame()
    {
        return true;
    }

    //public void Movescenemain()
    //{
    //    Debug.Log("게임 시작 버튼이 클릭되었습니다.");
    //    SceneManager.LoadScene("SampleScene");
    //}

    public void SetRestart()
    {
        Button button = GetComponent<Button>();

    }



    public void ExitGame()
    {
        Debug.Log("게임 종료 버튼이 클릭되었습니다.");

        // 실제 빌드된 게임(PC, 모바일 등)에서 게임을 종료시킵니다.
        Application.Quit();
    }
}