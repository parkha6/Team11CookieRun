using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private GameManager gameManager;
    public void Enter(Player player)
    {
        gameManager = GameManager.Instance;
    }
    public void Update(Player player)
    {
        //시작 시 RunState변경
        /*if (gameManager.IsStart)
            player.ChangeState(player.runState);*/

        //테스트 코드
        if (Input.GetKeyDown(KeyCode.N))
        {
            player.ChangeState(player.runState);
            gameManager.IsStart = true;
        }
    }

    public void Exit(Player player)
    {
        
    }
}
