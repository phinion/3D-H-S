using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitData
{
    public int damage;
    public Vector3 hitPoint;
    public Vector3 hitNormal;
    public IHurtbox hurtbox;
    public IHitDetector hitDetector;

    public bool Validate()
    {
        if (hurtbox != null)
            if (hurtbox.CheckHit(this))
                if (hurtbox.HurtResponder == null || hurtbox.HurtResponder.CheckHit(this))
                    if (hitDetector.HitResponder == null || hitDetector.HitResponder.CheckHit(this))
                        return true;

        return false;
    }
}

public enum HurtboxType
{
    Player = 1 << 0,
    Enemy = 1 << 1,
    Ally = 1 << 2
}
[System.Flags]
public enum HurtboxMask
{
    None    = 0,          //0000b
    Player  = 1 << 0,     //0001b
    Enemy   = 1 << 1,     //0010b
    Ally =   1 << 2       //0100b
}

public interface IHitResponder
{
    int Damage { get; }
    public bool CheckHit(HitData _data);

    public void Response(HitData _data);
}

public interface IHitDetector
{
    public IHitResponder HitResponder { get; set; }
    public void CheckHit();
}

public interface IHurtResponder
{
    public bool CheckHit(HitData _data);

    public void Response(HitData _data);
}

public interface IHurtbox
{
    public bool Active { get; }
    public GameObject Owner { get; }
    public Transform Transform { get; }
    public HurtboxType Type { get; }
    public IHurtResponder HurtResponder { get; set; }

    public bool CheckHit(HitData _hitData);
}
