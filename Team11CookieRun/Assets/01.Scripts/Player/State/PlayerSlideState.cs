using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerConstVar;
public class PlayerSlideState : IPlayerState
{
    public void EnterState(Player player)
    {
        ConditionSetting(player, true);
    }
    public void UpdateState(Player player)
    {
        player.MoveFoward();
        if (player.IsSlide == false)
            player.ChangeState(player.runState);
    }

    public void ExitState(Player player)
    {
        ConditionSetting(player, false);
        player.PlayerAnim.SetTrigger(SlideUpCondition);
    }

    public void ConditionSetting(Player player, bool isIn)
    {
        player.PlayerAnim.SetBool(SlideCondition, isIn);
    }
}
