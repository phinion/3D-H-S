using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour, IHurtbox
{
    [SerializeField] private bool active = true;
    [SerializeField] private GameObject owner = null;
    [SerializeField] private HurtboxType hurtboxType = HurtboxType.Enemy;
    private IHurtResponder hurtResponder;

    public bool Active { get => active; }
    public GameObject Owner { get => owner; }
    public Transform Transform { get => transform; }
    public HurtboxType Type { get => hurtboxType; }
    public IHurtResponder HurtResponder { get => hurtResponder; set => hurtResponder = value; }

    public bool CheckHit(HitData hitData)
    {
        if (hurtResponder == null)
            Debug.Log("No Responder");

        return true;
    }
}
