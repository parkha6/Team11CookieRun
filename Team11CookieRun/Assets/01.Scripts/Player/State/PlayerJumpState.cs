using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerConstVar;
public class PlayerJumpState : IPlayerState
{
    public void EnterState(Player player)
    {
        player.Jump();
        ConditionSetting(player, true);
    }
    public void UpdateState(Player player)
    {
        player.OnGravity();
        player.MoveFoward();

        if (player.IsGround)
            player.ChangeState(player.runState);
    }

    public void ExitState(Player player)
    {
        player.IsJump = false;
        ConditionSetting(player, false);
    }

    public void ConditionSetting(Player player, bool isIn)
    {
        player.PlayerAnim.SetBool(JumpCondition, isIn);
    }
}
