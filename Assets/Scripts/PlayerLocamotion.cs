using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerLocamotion : MonoBehaviour
{
    InputManager inputManager;
    AnimatorManager animatorManager;

    Vector3 moveDirection;
    Transform cameraObject;
    public Rigidbody rb;

    public float movementSpeed = 7f;
    public float movementMultiplier = 10f;
    public float rbDrag = 6f;

    public float rotationSpeed = 15f;

    public bool isGrounded = false;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        animatorManager = GetComponent<AnimatorManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * 0.1f));
    }

    public void DoPhysicsChecks()
    {
        isGrounded = Physics.Raycast(transform.position + (Vector3.up * 0.5f), Vector3.down, 0.5f + 0.1f);
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();

        //HandleMovementAnimation();
    }

    private Vector3 GetNormalizedMoveDirection()
    {
        Vector3 moveDirection;
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        return moveDirection;
    }

    private void HandleMovement()
    {
        Vector3 movementVelocity = GetNormalizedMoveDirection() * movementSpeed;

        //rb.velocity = movementVelocity;
        rb.AddForce(movementVelocity * movementMultiplier, ForceMode.Acceleration);
        ControlDrag();
    }

    private void ControlDrag()
    {
        rb.drag = rbDrag;
    }

    public void Dash(Vector3 _force, Vector3 _dir)
    {

    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    //private void HandleMovementAnimation()
    //{
    //    //Vector2 test = Vector2.zero;
    //    //test.y = (transform.forward * moveDirection.z).z;

    //    //Vector3 cameraDir = cameraObject.forward;
    //    //cameraDir.y = 0f;
    //    //Quaternion residentialShift = Quaternion.FromToRotation(transform.forward, moveDirection);

    //    //Vector3 moveDir = residentialShift * transform.forward;

    //    //float angle = Vector3.Angle(transform.forward, moveDirection);

    //    //float y = moveDirection.magnitude * Mathf.Cos(angle);
    //    //float x = moveDirection.magnitude * Mathf.Sin(angle);

    //    animatorManager.UpdateAnimatorValues(0f, y);
    //}
}
