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
        // gameOverUI�� null�� �ƴ� ��쿡�� ��Ȱ��ȭ�մϴ�. 
        // (��: Start ������ gameOverUI�� ���� ���� ����)
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
        uiManager.SetRestart();
       // PlayerPrefs.SetInt( ScoreLoader.LAST_SCORE_KEY , currentScore);
    }

    // --- ������� �߰��� �Լ��� ---

  
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// ������ �����մϴ�. (Exit ��ư�� �Ҵ�)
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("���� ���� ��ư Ŭ����");
        // ����: Application.Quit()�� ����Ƽ �����Ϳ����� �۵����� �ʰ�,
        // ���� ����� ���ӿ����� �۵��մϴ�.
        Application.Quit();
    }


    public void AddScore(int score)
    {
        currentScore += score;
        // uiManager.UpdateScore(currentScore);
        Debug.Log("Score: " + currentScore);
    }
}