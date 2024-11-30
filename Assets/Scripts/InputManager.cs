using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerLocamotion playerLocamotion;
    MovesManager movesManager;

    EnemyLockOn enemyTargetting;


    public float cameraInputX;
    public float cameraInputY;

    public float verticalInput;
    public float horizontalInput;

    public float movementInputDeadZone = 0.1f; 
    public Vector2 MovementInput
    {
        get
        {
            float x = Mathf.Abs(horizontalInput) > movementInputDeadZone ? horizontalInput : 0f;
            float y = Mathf.Abs(verticalInput) > movementInputDeadZone ? verticalInput : 0f;
            return new Vector2(x, y);
        }
    }
    public Vector2 CameraInput => new Vector2(cameraInputX, cameraInputY);

    public event Action OnAttack;
    public event Action OnRun;
    public event Action OnWalk;

    public bool attack;
    public bool heavyAttack;
    public bool defend;
    public bool dodge;
    public bool run;
    public bool walk = false;
    public bool jump;

    Vector2 movementInput;
    Vector2 cameraInput;

    private float moveAmount;
    private Vector2 lastMovementInput; // Stores the previous movement input
    private float neutralStateTimer = 0f; // Timer to track how long joystick is in neutral state
    private float neutralStateDelay = 0.2f; // Delay before registering neutral state (in seconds)

    private void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocamotion = GetComponent<PlayerLocamotion>();
        movesManager = GetComponent<MovesManager>();

        enemyTargetting = GetComponent<EnemyLockOn>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerCombat.Dodge.performed += i => dodge = true;
            playerControls.PlayerCombat.Dodge.canceled += i => dodge = false;

            playerControls.PlayerMovement.Jump.performed += i => jump = true;
            playerControls.PlayerMovement.Jump.canceled += i => jump = false;

            playerControls.PlayerCombat.Defend.performed += i => defend = true;
            playerControls.PlayerCombat.Defend.canceled += i => defend = false;
            //Debug.Log("Enable 1");
            playerControls.PlayerCombat.Attack.performed += i => PrimaryAttack(i);
            playerControls.PlayerCombat.Attack.canceled += i => PrimaryAttack(i);
            
            playerControls.PlayerCombat.HeavyAttack.performed += i => SecondaryAttack(i);
            playerControls.PlayerCombat.HeavyAttack.canceled += i => SecondaryAttack(i);
            //Debug.Log("Enable 2");
            playerControls.PlayerMovement.Run.performed += i =>
            {
                run = true;
                OnRun?.Invoke();
            };
            playerControls.PlayerMovement.Run.canceled += i =>
            {
                run = false;
                OnRun?.Invoke();
            };

            playerControls.PlayerMovement.Walk.performed += i =>
            {
                walk = !walk;
                OnWalk?.Invoke();
            };

            playerControls.PlayerCombat.TargetLock.performed += i => TargetLock(i);
        }

        playerControls.Enable();
    }

    private void TargetLock(InputAction.CallbackContext context)
    {
        enemyTargetting.TryTargetLock();
    }

    private void PrimaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attack = true;
            OnAttack?.Invoke();
        }
        else if (context.canceled)
        {
            attack = false;
        }
    }
    
    private void SecondaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            heavyAttack = true;
            OnAttack?.Invoke();
        }
        else if (context.canceled)
        {
            heavyAttack = false;
        }
    }


    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInput()
    {
        HandleMovementInput();
        //HandleJumpInput()
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        if (movementInput == Vector2.zero)
        {
            neutralStateTimer += Time.deltaTime;

            if (neutralStateTimer >= neutralStateDelay)
            {
                lastMovementInput = movementInput;
                moveAmount = 0f;
            }
        }
        else
        {
            neutralStateTimer = 0f;

            lastMovementInput = movementInput;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        }

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;
    }

}
