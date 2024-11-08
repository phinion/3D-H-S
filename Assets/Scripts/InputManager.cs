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
    public Vector2 movementInput { get; private set; }
    public Vector2 cameraInput { get; private set; }

    public float cameraInputX;
    public float cameraInputY;

    public float verticalInput;
    public float horizontalInput;

    public bool attack;
    public bool defend;
    public bool dodge;
    public bool run;
    public bool jump;

    private float moveAmount;

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
            //Debug.Log("Enable 2");
            playerControls.PlayerMovement.Run.performed += i => run = true;
            playerControls.PlayerMovement.Run.canceled += i => run = false;

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
            Vector2 movementDirection = movementInput;
            bool dashPressed = run;  
            bool dodgePressed = dodge;

            if (movesManager.CheckAvailableMoves(AttackType.light, movementDirection, dashPressed, dodgePressed))
            {
                //movesManager.DoNextMove();
            }
            /*else
            {
                movesManager.ClearAvailableMoves();
            }*/
        }
        else if (context.canceled)
        {
            attack = false;
        }
    }


    private void OnDisable()
    {
        playerControls.Disable();
        Debug.Log("Disable");
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

        //Debug.Log("movement Input: " + movementInput);

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        //Vector3 relativeDirection = CameraManager.instance.transform.InverseTransformDirection(playerLocamotion.GetNormalizedMoveDirection());

        //animatorManager.UpdateAnimatorValues(0f, verticalInput);
    }
}
