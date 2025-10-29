using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    static GameManager gameManager;

    public GameObject gameOverUI;

    public static GameManager Instance
    {
        get { return gameManager; }
    }

    private int currentScore = 0;
    UIManager uiManager;

 public UIManager UIManager
     {
         get { return uiManager; }
    }

    public static object instance { get; internal set; }

    private void Awake()
    {
        gameManager = this;
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        // gameOverUI가 null이 아닐 경우에만 비활성화합니다. 
        // (예: Start 씬에는 gameOverUI가 없을 수도 있음)
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
      // uiManager.UpdateScore(0);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
      //  uiManager.SetRestart();
       // PlayerPrefs.SetInt( ScoreLoader.LAST_SCORE_KEY , currentScore);
    }

    // --- 여기부터 추가된 함수들 ---

  
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// 게임을 종료합니다. (Exit 버튼에 할당)
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("게임 종료 버튼 클릭됨");
        // 참고: Application.Quit()는 유니티 에디터에서는 작동하지 않고,
        // 실제 빌드된 게임에서만 작동합니다.
        Application.Quit();
    }


    public void AddScore(int score)
    {
        currentScore += score;
        // uiManager.UpdateScore(currentScore);
        Debug.Log("Score: " + currentScore);
    }
}