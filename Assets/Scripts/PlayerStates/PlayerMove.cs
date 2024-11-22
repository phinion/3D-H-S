using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerState
{
    private bool UseRootAnim = false;
    bool isRun = false;
    bool isWalk = false;
    bool transitionToIdle = false;

    public PlayerMove(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        Vector3 moveDirection = player.playerLocamotion.GetNormalizedMoveDirection();

        // If no movement input, transition to idle
        if (moveDirection.sqrMagnitude > 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
        else
        {
            Debug.Log("NO MOVEMENTE RETURNING TO IDLE");
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void AnimationComboTrigger()
    {
        base.AnimationComboTrigger();
    }

    public override void OnAnimatorMove()
    {
        if (UseRootAnim)
            player.playerLocamotion.RootAnimMove(player.anim.deltaPosition);
    }

    public override void Enter()
    {
        base.Enter();
        UseRootAnim = false;
        transitionToIdle = false;
        player.movesManager.ResetAvailableMoves();

        player.inputManager.OnRun += OnRun;
        player.inputManager.OnWalk += OnWalk;

        OnRun();
        OnWalk();
    }

    void OnRun()
    {
        isRun = input.run;
    }

    void OnWalk()
    {
        isWalk = input.walk;
    }

    public override void Exit()
    {
        base.Exit();

        player.anim.SetBool(animBoolName, false);
        transitionToIdle = false;

        player.inputManager.OnRun -= OnRun;
        player.inputManager.OnWalk -= OnWalk;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //if (!isExitingState)
        //{
        if (player.movesManager.IsMoveAvailable())
        {
            player.movesManager.DoNextMove();
            stateMachine.ChangeState(player.attackState);
        }
        else if (input.dodge)
        {
            stateMachine.ChangeState(player.dodgeState);
        }
        else if (input.defend)
        {
            stateMachine.ChangeState(player.defendState);
        }
        else if (input.MovementInput == Vector2.zero && transitionToIdle == false)
        {
            Debug.Log("NO MOVEMENTE RETURNING TO IDLE IN LOGIC");
            // find out current speed and forward
            player.anim.SetBool(animBoolName, false);
            UseRootAnim = true;
            transitionToIdle = true;
        }
        else if (input.MovementInput != Vector2.zero && UseRootAnim == true)
        {
            player.anim.SetBool(animBoolName, true);
            UseRootAnim = false;
            transitionToIdle = false;
        }
        else if (input.jump && player.playerLocamotion.isGrounded)
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        /*if (transitionToIdle)
        {
            float movementMultiplier = player.anim.GetFloat("MovementMultiplier");
            var speed = player.playerLocamotion.runningSpeed * movementMultiplier;
            player.playerLocamotion.HandleMovement(speed, true);
            return;
        }
        */

        if (isRun)
        {
            player.playerLocamotion.HandleMovement(player.playerLocamotion.runningSpeed);
            player.playerLocamotion.HandleRotation(true, true);
        }
        else if (isWalk)
        {
            player.playerLocamotion.HandleAllMovement(player.playerLocamotion.walkingSpeed);
        }
        else
        {
            player.playerLocamotion.HandleAllMovement(player.playerLocamotion.movementSpeed);
        }

    }

    bool AreVectorsFacingAway(Vector3 vector1, Vector3 vector2, float angleThreshold = 135f)
    {
        vector1.Normalize();
        vector2.Normalize();

        float dotProduct = Vector3.Dot(vector1, vector2);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        return angle > (180f - angleThreshold);
    }

}
