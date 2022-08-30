using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDummy : MonoBehaviour, IHurtResponder
{
    [SerializeField] private Rigidbody rb;

    private List<Hurtbox> hurtboxes = new List<Hurtbox>();

    private void Start()
    {

        hurtboxes = new List<Hurtbox>(GetComponentsInChildren<Hurtbox>());
        foreach(Hurtbox _hurtbox in hurtboxes)
        {
            _hurtbox.HurtResponder = this;
        }

    }

    bool IHurtResponder.CheckHit(HitData _data)
    {
        return true;
    }

    void IHurtResponder.Response(HitData _data)
    {
        Debug.Log("Hurt Response");
    }
}
