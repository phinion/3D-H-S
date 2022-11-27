using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { light = 0, heavy = 1 }

[System.Serializable]
public class Attack
{
    public string name;
    public float length;
}

[System.Serializable]
public class ComboInput
{
    public AttackType type;
    // movement input for precise combos

    public ComboInput(AttackType _t)
    {
        type = _t;
    }

    public bool IsSameAs(ComboInput _i)
    {
        return (type == _i.type);
    }
}

[System.Serializable]

public class AttackCombo
{
    public Attack comboAttack;
    public List<ComboInput> inputs;

    int curInput = 0;

    public bool ContinueCombo(ComboInput _i)
    {
        if (_i.IsSameAs(inputs[curInput])) // add && i.movement = inputs[cuInput].movement
        {
            curInput++;
            if (curInput >= inputs.Count) //Finishes the inputs and we should do the attack
            {
                //invoke event
                ResetCombo();
            }
            return true;
        }
        else
        {
            ResetCombo();
            return false;
        }
    }

    public ComboInput CurrentComboInput()
    {
        if (curInput >= inputs.Count) return null;
        return inputs[curInput];
    }

    public void ResetCombo()
    {
        curInput = 0;
    }
}
