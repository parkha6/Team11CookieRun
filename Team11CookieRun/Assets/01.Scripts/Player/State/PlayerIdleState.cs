using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    public void Enter(Player player)
    {
        
    }
    public void Update(Player player)
    {
        //시작 시 RunState변경
        if (Input.GetKeyDown(KeyCode.N))
            player.ChangeState(player.runState);
    }

    public void Exit(Player player)
    {
        
    }
}
