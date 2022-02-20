using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerStateMachine stateMachine;

    public string CurrentState;

    public InputManager inputManager;
    CameraManager cameraManager;
    public PlayerLocamotion playerLocamotion;
    public Animator anim;

    public PlayerIdle idleState;
    public PlayerMove moveState;
    public PlayerFall fallState;

    public PlayerAttack attackState;
    public PlayerDodge dodgeState;


    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        //inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        //playerLocamotion = GetComponent<PlayerLocamotion>();

        //anim = GetComponent<Animator>();

        SetupStates();
    }

    private void SetupStates()
    {
        idleState = new PlayerIdle(this, stateMachine, "");
        moveState = new PlayerMove(this, stateMachine, "move");
        attackState = new PlayerAttack(this, stateMachine, "attack");
        dodgeState = new PlayerDodge(this, stateMachine, "dodge");

        //fallState = new PlayerFall(this, stateMachine, "fall");
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        inputManager.HandleAllInput();
        stateMachine.CurrentState.LogicUpdate();

        CurrentState = stateMachine.CurrentState.ToString();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
    }

    public void SetAnimationFinishedTrigger() => stateMachine.CurrentState.AnimationFinishTrigger();

}
