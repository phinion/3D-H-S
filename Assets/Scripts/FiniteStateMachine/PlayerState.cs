using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerManager player;
    protected PlayerStateMachine stateMachine;
    protected InputManager input;
    protected Animator anim;

    protected bool isAnimationFinished;
    protected bool canCombo;
    protected bool isExitingState;

    protected float startTime;

    protected string animBoolName;

    public PlayerState(PlayerManager _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        input = player.inputManager;
        anim = this.player.anim;
    }

    public virtual void Enter()
    {
        DoChecks();
        AnimationTrigger();
        startTime = Time.time;

       // Debug.Log(this.ToString());

        isAnimationFinished = false;
        isExitingState = false;
        canCombo = false;
    }

    public virtual void Exit()
    {
        //player.anim.SetBool(animBoolName, false);
        isExitingState = true;
        canCombo = false;
    }

    public virtual void LogicUpdate()
    {
        /*if (!player.playerLocamotion.isGrounded)
        {
            stateMachine.ChangeState(player.fallState);
        }*/
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {
        player.playerLocamotion.DoPhysicsChecks();
    }

    public virtual void AnimationTrigger()
    {
        player.anim.SetBool(animBoolName, true);
    }

    public virtual void OnAnimatorMove()
    {
        //override without the base so that values are not added;
        //player.playerLocamotion.RootAnimMove(player.anim.deltaPosition);
    }

    public virtual void AnimationFinishTrigger() 
    {
        isAnimationFinished = true;
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationComboTrigger()
    {
        //player.anim.SetBool("combo", true);
        canCombo = true;
    }

    public bool ExitingState
    {
        get => isExitingState;
    }
}
