using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_HitResponder : MonoBehaviour, IHitResponder
{
    [SerializeField] private bool attack;
    [SerializeField] private int damage = 10;
    [SerializeField] private Hitbox hitbox;

    int IHitResponder.Damage { get => damage; }

    private void Start()
    {
        hitbox.HitResponder = this;
    }

    private void Update()
    {
        if (attack)
        {
            hitbox.CheckHit();
        }
    }

    bool IHitResponder.CheckHit(HitData _data)
    {
        return true;
    }
    void IHitResponder.Response(HitData _data)
    {

    }
}
