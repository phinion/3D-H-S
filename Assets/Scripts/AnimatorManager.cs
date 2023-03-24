using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    PlayerManager playerManager;
    Animator animator;
    int horizontal;
    int vertical;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        animator = GetComponentInChildren<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalMovement,float verticalMovement)
    {
        //Animation Snapping
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            snappedHorizontal = 0.5f;
        } 
        else if (horizontalMovement > 0.55f)
        {
            snappedHorizontal = 1f;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            snappedHorizontal = -1f;
        }
        else
        {
            snappedHorizontal = 0f;
        }
        #endregion
        #region Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            snappedVertical = 1f;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            snappedVertical = -1f;
        }
        else
        {
            snappedVertical = 0f;
        }
        #endregion

        //Debug.Log("snapped Vertical " + snappedVertical);

        animator.SetFloat(horizontal, snappedHorizontal, 0.1f,Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);

    }

    private void OnAnimatorMove()
    {
        playerManager.OnAnimatorMove();
    }

    public void SetAnimationFinishedTrigger() => playerManager.SetAnimationFinishedTrigger();
    public void SetAnimationComboTrigger() => playerManager.SetAnimationComboTrigger();

}
