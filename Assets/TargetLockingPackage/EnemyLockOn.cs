using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyLockOn : MonoBehaviour
{
    public bool enemyLocked;
    public Transform currentTarget;
    //Animator anim;

    [SerializeField] CinemachineFreeLook followCamera;

    [SerializeField] LayerMask targetLayers;
    [SerializeField] Transform enemyTarget_Locator;

    [Tooltip("StateDrivenMethod for Switching Cameras")]
    [SerializeField] Animator cinemachineAnimator;

    [Header("Settings")]
    [SerializeField] bool zeroVert_Look;
    [SerializeField] float noticeZone = 10;
    [SerializeField] float lookAtSmoothing = 2;
    [Tooltip("Angle_Degree")] [SerializeField] float maxNoticeAngle = 60;
    [SerializeField] float crossHair_Scale = 0.1f;


    Transform cam;

    

    float currentYOffset;
    Vector3 pos;

    //[SerializeField] CameraFollow camFollow;
    [SerializeField] Transform lockOnCanvas;

    void Start()
    {
        //anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        lockOnCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        //camFollow.lockedTarget = enemyLocked;
        //defMovement.lockMovement = enemyLocked;

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    if (currentTarget)
        //    {
        //        //If there is already a target, Reset.
        //        ResetTarget();
        //        return;
        //    }

        //    if (currentTarget = ScanNearBy()) FoundTarget(); else ResetTarget();
        //}

        if (enemyLocked)
        {
            if (!TargetOnRange()) ResetTarget();
            LookAtTarget();
        }

    }

    public void TryTargetLock()
    {
        if (currentTarget)
        {
            //If there is already a target, Reset.
            ResetTarget();
            return;
        }

        if (currentTarget = ScanNearBy()) FoundTarget(); else ResetTarget();
    }


    void FoundTarget()
    {
        lockOnCanvas.gameObject.SetActive(true);
        //anim.SetLayerWeight(1, 1);
        cinemachineAnimator.Play("TargetCamera");
        enemyLocked = true;

        followCamera.LookAt = currentTarget;
    }

    void ResetTarget()
    {
        lockOnCanvas.gameObject.SetActive(false);
        currentTarget = null;
        enemyLocked = false;
        //anim.SetLayerWeight(1, 0);

        followCamera.LookAt = followCamera.Follow;

        cinemachineAnimator.Play("FollowCamera");
    }


    private Transform ScanNearBy()
    {
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;
        if (nearbyTargets.Length <= 0) return null;

        for (int i = 0; i < nearbyTargets.Length; i++)
        {
            Vector3 dir = nearbyTargets[i].transform.position - cam.position;
            dir.y = 0;
            float _angle = Vector3.Angle(cam.forward, dir);

            if (_angle < closestAngle)
            {
                closestTarget = nearbyTargets[i].transform;
                closestAngle = _angle;
            }
        }

        if (!closestTarget) return null;

        //float h1 = closestTarget.GetComponent<CapsuleCollider>().height;
        //float h2 = closestTarget.localScale.y;
        //float h = h1 * h2;
        //float half_h = (h / 2) / 2;
        //currentYOffset = h - half_h;

        //if (zeroVert_Look && currentYOffset > 1.6f && currentYOffset < 1.6f * 3) currentYOffset = 1.6f;

        currentYOffset = closestTarget.transform.lossyScale.y * (2f / 3f);

        //currentYOffset = 1.6f;

        Vector3 tarPos = closestTarget.position + new Vector3(0, currentYOffset, 0);

        if (Blocked(tarPos)) return null;

        return closestTarget;
    }

    bool Blocked(Vector3 t)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * 0.5f, t, out hit))
        {
            if (!hit.transform.CompareTag("Enemy")) return true;
        }
        return false;
    }

    bool TargetOnRange()
    {
        float dis = (transform.position - pos).magnitude;
        if (dis / 2 > noticeZone) return false; else return true;
    }


    private void LookAtTarget()
    {
        if (currentTarget == null)
        {
            ResetTarget();
            return;
        }
        //pos = currentTarget.position + new Vector3(0, currentYOffset, 0);
        pos = currentTarget.position;
        lockOnCanvas.position = pos;
        lockOnCanvas.localScale = Vector3.one * ((cam.position - pos).magnitude * crossHair_Scale);

        enemyTarget_Locator.position = pos;

        //Vector3 dir = currentTarget.position - transform.position;
        //dir.y = 0;
        //Quaternion rot = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * lookAtSmoothing);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone);
    }
}
