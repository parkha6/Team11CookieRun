using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private GameManager gameManager;
    private PlayerInputManager inputManger;
    public void EnterState(Player player)
    {
        gameManager = GameManager.Instance;
        inputManger = PlayerInputManager.Instance;
        inputManger.DisableInput();
    }
    public void UpdateState(Player player)
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

    public void ExitState(Player player)
    {
        inputManger.EnableInput();
    }

    public void ConditionSetting(Player player, bool isIn)
    {
    }
}
