using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerLocamotion : MonoBehaviour
{
    public Rigidbody rb;

    InputManager inputManager;
    AnimatorManager animatorManager;
    EnemyLockOn enemyTargetting;
    Transform cameraObject;

    [Header("Movement Settings")]
    public float movementSpeed = 7f;
    public float runningSpeed = 14f;
    
    public float rotationSpeed = 15f;
    
    public float jumpForce = 5f;     
    
    public float movementMultiplier = 10f;
    public float groundDrag = 6f;
    public float airDrag = 0.5f;
    public float fallMultiplier = 2.5f;
    
    [Header("Ground Check Settings")] 
    public float groundedDistanceCheck = 0.2f;
    public bool isGrounded = false;

    public float animatorSpeedModifier = 0.5f;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;

        enemyTargetting = GetComponent<EnemyLockOn>();
    }

    private void FixedUpdate()
    {
        HandleDragAndGravity();
        HandleMovementAnimation();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * groundedDistanceCheck));
    }

    public void DoPhysicsChecks()
    {
        isGrounded = Physics.Raycast(transform.position + (Vector3.up * 0.5f), Vector3.down,
            0.5f + groundedDistanceCheck);
    }

    public void HandleAllMovement(float _speed)
    {
        HandleMovement(_speed);
        HandleRotation();
    }

    public Vector3 GetNormalizedMoveDirection()
    {
        Vector3 moveDir;
        moveDir = cameraObject.forward * inputManager.verticalInput;
        moveDir = moveDir + cameraObject.right * inputManager.horizontalInput;
        moveDir.Normalize();
        moveDir.y = 0;
        return moveDir;
    }

    #region Movement and Rotation given input

    public void HandleMovement(float _speed)
    {
        Vector3 movementVelocity = GetNormalizedMoveDirection() * _speed;

        //rb.velocity = movementVelocity;
        rb.AddForce(movementVelocity * movementMultiplier, ForceMode.Acceleration);

        //HandleMovementAnimation();
    }

    private void HandleDragAndGravity()
    {
        if (isGrounded)
        {
            // Apply ground drag
            rb.drag = groundDrag;
        }
        else
        {
            // Apply air drag
            rb.drag = airDrag;

            // Apply additional gravity when falling
            if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * (fallMultiplier - 1) * Physics.gravity.magnitude, ForceMode.Acceleration);
            }
        }
    }

    public void HandleRotation(bool useDefault = true, bool ignoreTarget = false)
    {
        if (enemyTargetting.enemyLocked && !ignoreTarget)
        {
            LockedRotation();
        }
        else if (useDefault)
        {
            BasicRotation();
        }
    }

    private void BasicRotation()
    {
        Vector3 targetDirection = GetNormalizedMoveDirection();

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation =
            Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void LockedRotation()
    {
        Vector3 targetDirection = (enemyTargetting.currentTarget.transform.position - transform.position).normalized;
        targetDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation =
            Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    #endregion

    #region Movement and Rotation given direction

    public void HandleMovement(Vector3 _normalizedTargetDirection)
    {
        Vector3 movementVelocity = _normalizedTargetDirection * movementSpeed;

        //rb.velocity = movementVelocity;
        rb.AddForce(movementVelocity * movementMultiplier, ForceMode.Acceleration);

        //HandleMovementAnimation();
    }

    public void HandleRotation(Vector3 _normalizedTargetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(_normalizedTargetDirection);
        Quaternion playerRotation =
            Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public void HandleAllMovement(Vector3 _normalizedTargetDirection)
    {
        HandleMovement(_normalizedTargetDirection);
        HandleRotation(_normalizedTargetDirection);
    }

    #endregion

    public void Dash(float _force, Vector3 _normalizedTargetDirection)
    {
        //Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //transform.rotation = targetRotation;

        rb.AddForce(_force * movementMultiplier * _normalizedTargetDirection, ForceMode.Acceleration);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 5f);
    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reset y velocity before jumping
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void HandleMovementAnimation()
    {
        //Vector2 test = Vector2.zero;
        //test.y = (transform.forward * moveDirection.z).z;

        //Vector3 cameraDir = cameraObject.forward;
        //cameraDir.y = 0f;
        //Quaternion residentialShift = Quaternion.FromToRotation(transform.forward, moveDirection);

        //Vector3 moveDir = residentialShift * transform.forward;

        //float angle = Vector3.Angle(transform.forward, moveDirection);

        //float y = moveDirection.magnitude * Mathf.Cos(angle);
        //float x = moveDirection.magnitude * Mathf.Sin(angle);

        Vector3 moveDir = GetNormalizedMoveDirection();

        moveDir = transform.InverseTransformDirection(moveDir);

        moveDir *= (rb.velocity.magnitude * animatorSpeedModifier);

        animatorManager.UpdateAnimatorValues(moveDir.x, moveDir.z);
    }

    public void RootAnimMove(Vector3 animDeltaPosition)
    {
        transform.position += new Vector3(animDeltaPosition.x, 0, animDeltaPosition.z);
    }
}