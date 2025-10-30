using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : IPlayerState
{
    public void Enter(Player player)
    {
        player.Jump();
        JumpState(player, true);
    }
    public void Update(Player player)
    {
        player.OnGravity();
        player.MoveFoward();

        if (player.IsGround)
            player.ChangeState(player.runState);
    }

    public void Exit(Player player)
    {
        player.IsJump = false;
        JumpState(player, false);
    }

    private void JumpState(Player player, bool isIn)
    {
        player.PlayerAnim.SetBool("IsJump", isIn);
    }
}
