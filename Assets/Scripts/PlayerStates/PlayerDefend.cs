using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefend : PlayerState
{
    public PlayerDefend(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(player.idleState);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        player.anim.SetInteger("comboCount", 0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!input.defend)
        {
            player.anim.SetInteger("comboCount",1);
        }
    }
    
    public override void OnAnimatorMove()
    {
        player.playerLocamotion.RootAnimMove(player.anim.deltaPosition);
    }
}
