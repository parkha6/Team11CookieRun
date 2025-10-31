using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerConstVar;

public class PlayerDeathState : IPlayerState
{
    private readonly WaitForSeconds dieDelay = new WaitForSeconds(2f);
    public void EnterState(Player player)
    {
        ConditionSetting(player, true);
        //StopPhysics(player);
        DisableInput();
        player.StartCoroutine(Die());
    }

    public void UpdateState(Player player)
    {
        
    }
    public void ExitState(Player player)
    {
        
    }
    public void ConditionSetting(Player player, bool isIn)
    {
        player.Speed = 0f;
        player.IsInvincible = true;
        player.IsDie = isIn;
        player.PlayerAnim.SetTrigger(DieCondition);
    }

    public void StopPhysics(Player player)
    {
        if (player.TryGetComponent(out Rigidbody2D rb))
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        if (player.TryGetComponent(out Collider2D col))
            col.enabled = false;
    }

    private void DisableInput()
    {
        PlayerInputManager playerInputManager = PlayerInputManager.Instance;
        playerInputManager.DisableInput();
    }
    IEnumerator Die()
    {
        yield return dieDelay;
        //다이 화면 나옴
    }
}
