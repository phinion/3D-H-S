using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IHitResponder
{
    PlayerStateMachine stateMachine;

    public string CurrentState;
    public bool IsGrounded;

    public MovesManager movesManager;
    public InputManager inputManager;
    CameraManager cameraManager;
    public PlayerLocamotion playerLocamotion;
    public Animator anim;

    public PlayerIdle idleState;
    public PlayerMove moveState;
    public PlayerFall fallState;
    public PlayerJump jumpState;
    

    public PlayerAttack attackState;
    public PlayerDefend defendState;
    public PlayerDodge dodgeState;

    [Header("Attacking")]
    [SerializeField] private int damage = 10;
    [HideInInspector] public Hitbox hitbox;
    [HideInInspector] public List<GameObject> objectsHit = new List<GameObject>();

    int IHitResponder.Damage { get => damage; }

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
        defendState = new PlayerDefend(this, stateMachine, "defend");

        jumpState = new PlayerJump(this, stateMachine, "jump");
        fallState = new PlayerFall(this, stateMachine, "fall");
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);

        hitbox = GetComponentInChildren<Hitbox>();
        hitbox.HitResponder = this;
    }

    private void Update()
    {
        inputManager.HandleAllInput();
        stateMachine.CurrentState.LogicUpdate();

        CurrentState = stateMachine.CurrentState.ToString();
        IsGrounded = playerLocamotion.isGrounded;
    }
    private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        //cameraManager.HandleAllCameraMovement();
    }

    public void OnAnimatorMove()
    {
        //Vector3 rootMotionVelocity = anim.deltaPosition / Time.deltaTime;
        //Vector3 rootMotionPosition = transform.position - anim.rootPosition;
        //transform.position += anim.deltaPosition;
        //transform.rotation *= anim.deltaRotation;
        //GetComponent<Rigidbody>().velocity = rootMotionVelocity;
        stateMachine.CurrentState.OnAnimatorMove();
    }

    public void SetAnimationFinishedTrigger() => stateMachine.CurrentState.AnimationFinishTrigger();
    public void SetAnimationComboTrigger() => stateMachine.CurrentState.AnimationComboTrigger();

    bool IHitResponder.CheckHit(HitData _data)
    {
        if (_data.hurtbox.Owner == gameObject)              { return false; }
        else if (objectsHit.Contains(_data.hurtbox.Owner))  { return false; }
        else                                                { return true; }
    }

    void IHitResponder.Response(HitData _data)
    {
        objectsHit.Add(_data.hurtbox.Owner);
    }
}
