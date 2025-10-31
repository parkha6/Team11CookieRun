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
        player.IsGround = ConfirmGround(player);
        if (player.IsSlide == false)
            player.ChangeState(player.runState);
        else if (player.IsGround == false)
        {
            player.OnGravity(player.GroundGravity);
            player.FallPlayer();
        }
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

    private bool ConfirmGround(Player player)
    {
        Vector3 pos = player.transform.position;
        Vector2 box = player.BoxSize;
        float distance = player.RayDistance;
        LayerMask layerMask = player.GroundLayer;
        return Physics2D.BoxCast(pos, box, 0f, Vector2.down, distance, layerMask);
    }
}
