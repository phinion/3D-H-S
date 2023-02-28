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

    public Vector2 movementInput { get; private set; }
    public Vector2 cameraInput { get; private set; }

    public float cameraInputX;
    public float cameraInputY;

    public float verticalInput;
    public float horizontalInput;

    public bool attack;
    public bool dodge;
    //public bool run;

    private float moveAmount;

    private void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocamotion = GetComponent<PlayerLocamotion>();
        movesManager = GetComponent<MovesManager>();
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
            Debug.Log("Enable 1");
            playerControls.PlayerCombat.Attack.performed += i => PrimaryAttack(i);
            playerControls.PlayerCombat.Attack.canceled += i => PrimaryAttack(i);
            Debug.Log("Enable 2");
            //playerControls.PlayerMovement.Run.performed += i => run = true;
            //playerControls.PlayerMovement.Run.canceled += i => run = false;
        }

        playerControls.Enable();
    }

    private void PrimaryAttack(InputAction.CallbackContext context)
    {
        //switch (context.interaction)
        //{
        //    case HoldInteraction:
        //        break;
        //    default:
        //        break;
        //}

        if (context.performed)
        {
            //Check if hold threshhold is reached then
            // call primary hold

            //attack = true;


            if (movesManager.CheckAvailableMoves(AttackType.light))
            {
                movesManager.DoMove();
            }




            //call activationcheck for attack
        }
        else if (context.canceled)
        {
            attack = false;
        }

        //if (context.interaction is HoldInteraction)
        //{
        //    Debug.Log("Hold performed");
        //}
        //if (context.interaction is SlowTapInteraction)
        //{
        //    Debug.Log("Slow Tap performed");
        //}
        //if (context.interaction is TapInteraction)
        //{
        //    Debug.Log("Tap performed");
        //}

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
        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }
}
