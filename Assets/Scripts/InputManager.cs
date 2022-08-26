using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerLocamotion playerLocamotion;

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

            playerControls.PlayerCombat.Attack.performed += i => attack = true;
            playerControls.PlayerCombat.Attack.canceled += i => attack = false;

            //playerControls.PlayerMovement.Run.performed += i => run = true;
            //playerControls.PlayerMovement.Run.canceled += i => run = false;
        }

        playerControls.Enable();
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

        //Debug.Log("movement Input: " + movementInput);

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }
}
