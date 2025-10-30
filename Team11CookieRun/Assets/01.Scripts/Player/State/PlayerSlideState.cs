using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerConstVar;
public class PlayerSlideState : IPlayerState
{
    public void Enter(Player player)
    {
        SlideState(player, true);
    }
    public void Update(Player player)
    {
        player.MoveFoward();
        if (player.IsSlide == false)
            player.ChangeState(player.runState);
    }

    public void Exit(Player player)
    {
        SlideState(player, false);
        player.PlayerAnim.SetTrigger(SlideUpCondition);
    }

    private void SlideState(Player player, bool isIn)
    {
        player.PlayerAnim.SetBool(SlideCondition, isIn);
    }
}
