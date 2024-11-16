using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    PlayerManager playerManager;
    Animator animator;
    int horizontal;
    int vertical;
    int magnitude;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        animator = GetComponentInChildren<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        magnitude = Animator.StringToHash("moveMagnitude");
    }

    public void UpdateAnimatorValues(float horizontalMovement,float verticalMovement, float moveMagnitude = 0f)
    {
        //Animation Snapping
        float snappedHorizontal;
        float snappedVertical;

        //#region Snapped Horizontal
        //if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        //{
        //    snappedHorizontal = 0.5f;
        //} 
        //else if (horizontalMovement > 0.55f)
        //{
        //    snappedHorizontal = 1f;
        //}
        //else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        //{
        //    snappedHorizontal = -0.5f;
        //}
        //else if (horizontalMovement < -0.55f)
        //{
        //    snappedHorizontal = -1f;
        //}
        //else
        //{
        //    snappedHorizontal = 0f;
        //}
        //#endregion
        //#region Snapped Vertical
        //if (verticalMovement > 0 && verticalMovement < 0.55f)
        //{
        //    snappedVertical = 0.5f;
        //}
        //else if (verticalMovement > 0.55f)
        //{
        //    snappedVertical = 1f;
        //}
        //else if (verticalMovement < 0 && verticalMovement > -0.55f)
        //{
        //    snappedVertical = -0.5f;
        //}
        //else if (verticalMovement < -0.55f)
        //{
        //    snappedVertical = -1f;
        //}
        //else
        //{
        //    snappedVertical = 0f;
        //}
        //#endregion

        //Debug.Log("snapped Horizontal " + snappedHorizontal + "snapped Vertical " + snappedVertical);

        snappedHorizontal = horizontalMovement;
        snappedVertical = verticalMovement;

        animator.SetFloat(horizontal, snappedHorizontal, 0.1f,Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
        animator.SetFloat(magnitude, moveMagnitude, 0.1f, Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        playerManager.OnAnimatorMove();
    }

    public void SetAnimationFinishedTrigger() => playerManager.SetAnimationFinishedTrigger();
    public void SetAnimationComboTrigger() => playerManager.SetAnimationComboTrigger();

}
