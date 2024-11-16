using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : PlayerState
{
    Vector3 normalizedMoveDir;
    bool isRun = false;

    public PlayerDodge(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        anim.ResetTrigger("dodge");

        //if (input.movementInput == Vector2.zero)
        //{
        //    stateMachine.ChangeState(player.idleState);
        //}
        //else
        //{
        stateMachine.ChangeState(player.idleState);
        //}

    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        //inputDir = new Vector2(player.inputManager.horizontalInput, player.inputManager.verticalInput);
        normalizedMoveDir = player.playerLocamotion.GetNormalizedMoveDirection();

        Vector3 dodgeDir = player.gameObject.transform.InverseTransformDirection(normalizedMoveDir);

        if (Mathf.Abs(dodgeDir.z) > Mathf.Abs(dodgeDir.x))
        {
            if (dodgeDir.z > 0)
                player.anim.SetInteger("comboCount", 0);
            else
                player.anim.SetInteger("comboCount", 1);
        }
        else
        {
            if (dodgeDir.x < 0)
                player.anim.SetInteger("comboCount", 2);
            else
                player.anim.SetInteger("comboCount", 3);
        }

        isRun = input.run;
    }

    public override void Exit()
    {
        base.Exit();

        normalizedMoveDir = Vector3.zero;
        player.anim.SetInteger("comboCount", 0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //if (canCombo)
        //{
        //    if (input.dodge)
        //    {
        //        stateMachine.ChangeState(player.dodgeState);
        //    }
        //}
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!canCombo)
        {
            player.playerLocamotion.Dash(player.playerLocamotion.dodgeForce, normalizedMoveDir);
        }
        else
        {
            player.playerLocamotion.HandleMovement(normalizedMoveDir);
        }
        if(!isRun)
        {
            player.playerLocamotion.HandleRotation(false);
        }
    }
}
