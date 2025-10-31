using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void EnterState(Player player);
    void UpdateState(Player player);
    void ExitState(Player player);

    void ConditionSetting(Player player, bool isIn);
}
