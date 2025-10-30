using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerConstVar;

public class PlayerRunState : IPlayerState
{
    public void Enter(Player player)
    {
        RunState(player,true);
    }
    public void Update(Player player)
    {
        player.MoveFoward();
        if (player.IsJump)
            player.ChangeState(player.jumpState);
        else if(player.IsSlide)
            player.ChangeState(player.slideState);
    }

    public void Exit(Player player)
    {
        player.IsRun = false;
        RunState(player, false);      
    }


    private void RunState(Player player, bool isIn)
    {
        player.PlayerAnim.SetBool(RunCondition, isIn);
    }
}
