using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : PlayerState
{

    bool useAnimY = false;
    bool gravityDisabled = false;

    public PlayerAttack(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.objectsHit.Clear();

        var currentAttack = player.movesManager.CurrentAttack();
        if (currentAttack != null)
        {
            player.playerLocamotion.DashForward(currentAttack.forwardImpulse);

            useAnimY = currentAttack.useRootY;

            if (currentAttack.isLauncher)
            {
                gravityDisabled = true;
                player.playerLocamotion.rb.useGravity = false;
                player.playerLocamotion.rb.velocity = Vector3.zero;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (gravityDisabled)
        {
            gravityDisabled = false;
            player.playerLocamotion.rb.useGravity = true;
        }

        player.anim.ResetTrigger(animBoolName);
    }

    public override void AnimationFinishTrigger()
    {
        isAnimationFinished = true;

        player.movesManager.ClearAvailableMoves();
        stateMachine.ChangeState(player.idleState);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        player.anim.SetTrigger(animBoolName);
    }


    public override void DoChecks()
    {
        base.DoChecks();

        if (!canCombo)
        {
            player.hitbox.CheckHit();
        }
    }

    public override void OnAnimatorMove()
    {
        if (player.movesManager.CurrentAttack() is not null)
        {
            var currentAttack = player.movesManager.CurrentAttack();

            if (currentAttack.isLauncher && gravityDisabled && !canCombo)
            {
                Vector3 launchVelocity = new Vector3(0, currentAttack.launchForceY, 0);
                float verticalCurveValue = player.anim.GetFloat("VerticalCurve");
                player.playerLocamotion.rb.velocity = launchVelocity * verticalCurveValue;
            }
        }

        Vector3 movement = player.anim.deltaPosition;
        movement.y = useAnimY ? movement.y : 0;
        player.playerLocamotion.RootAnimMove(movement, useAnimY);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (canCombo)
        {
            if (player.movesManager.IsMoveAvailable())
            {
                player.movesManager.DoNextMove();
                stateMachine.ChangeState(this);
            }
            else if (player.movesManager.CurrentAttackMaintainsMomentum() && input.MovementInput != Vector2.zero)
            {
                player.movesManager.ClearAvailableMoves();
                anim.CrossFade("Movement", 0.2f);
                stateMachine.ChangeState(player.moveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
