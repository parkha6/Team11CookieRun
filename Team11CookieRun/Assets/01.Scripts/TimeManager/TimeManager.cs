using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] float interval;
    [SerializeField] float upSpeed;
    public Player player;

    private float timer = 0f;


    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (gameManager.IsPause || gameManager.IsStart == false) return;

        timer += Time.deltaTime;
        if(timer >= interval)
        {
            timer = 0f;
            player.SpeedUp(upSpeed);
        }        
    }
}
