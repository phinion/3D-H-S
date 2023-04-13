using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerLocamotion : MonoBehaviour
{
    InputManager inputManager;
    AnimatorManager animatorManager;

    EnemyLockOn enemyTargetting;

    Vector3 moveDirection;
    Transform cameraObject;
    public Rigidbody rb;

    public float movementSpeed = 7f;
    public float runningSpeed = 14f;
    public float movementMultiplier = 10f;
    public float rbDrag = 6f;

    public float rotationSpeed = 15f;

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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * groundedDistanceCheck));
    }

    public void DoPhysicsChecks()
    {
        isGrounded = Physics.Raycast(transform.position + (Vector3.up * 0.5f), Vector3.down, 0.5f + groundedDistanceCheck);
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
    private void HandleMovement(float _speed)
    {
        Vector3 movementVelocity = GetNormalizedMoveDirection() * _speed;

        //rb.velocity = movementVelocity;
        rb.AddForce(movementVelocity * movementMultiplier, ForceMode.Acceleration);
        ControlDrag();

        HandleMovementAnimation();
    }

    private void ControlDrag()
    {
        rb.drag = rbDrag;
    }
    public void HandleRotation()
    {
        if (enemyTargetting.enemyLocked)
        {
            LockedRotation();
        }
        else
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
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void LockedRotation()
    {
        Vector3 targetDirection = (enemyTargetting.currentTarget.transform.position - transform.position).normalized;
        targetDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    #endregion

    #region Movement and Rotation given direction
    public void HandleMovement(Vector3 _normalizedTargetDirection)
    {
        Vector3 movementVelocity = _normalizedTargetDirection * movementSpeed;

        //rb.velocity = movementVelocity;
        rb.AddForce(movementVelocity * movementMultiplier, ForceMode.Acceleration);
        ControlDrag();

        HandleMovementAnimation();
    }

    // might need to change these to public so that we can call when we are targetting enemy but moving separately
    public void HandleRotation(Vector3 _normalizedTargetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(_normalizedTargetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

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
        transform.position += new Vector3(animDeltaPosition.x,0, animDeltaPosition.z);
    }
}
