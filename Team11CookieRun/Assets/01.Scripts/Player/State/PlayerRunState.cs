using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerConstVar;

public class PlayerRunState : IPlayerState
{
    public void EnterState(Player player)
    {
        ConditionSetting(player,true);
    }
    public void UpdateState(Player player)
    {
        player.MoveFoward();
        if (player.IsJump)
            player.ChangeState(player.jumpState);
        else if(player.IsSlide)
            player.ChangeState(player.slideState);
    }

    public void ExitState(Player player)
    {
        player.IsRun = false;
        ConditionSetting(player, false);      
    }

    public void ConditionSetting(Player player, bool isIn)
    {
        player.PlayerAnim.SetBool(RunCondition, isIn);
    }
}
