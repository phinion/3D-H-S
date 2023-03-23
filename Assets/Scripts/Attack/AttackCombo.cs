using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { light = 0, heavy = 1 }

[System.Serializable]
public class Attack
{
    public string name;
    public string anim;
    public ComboInput input;
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
        if (_i != null)
        {
            return (type == _i.type);
        }
        else
        {
            return false;
        }
    }
}

[System.Serializable]

public class AttackCombo
{
    public string name;

    public List<Attack> combo;
    //public List<ComboInput> inputs;

    public bool ContinueCombo(ComboInput _i, int comboCount)
    {
        if (_i.IsSameAs(CurrentComboInput(comboCount))) // add && i.movement = inputs[cuInput].movement
        {

            //if (curInput >= inputs.Count) //Finishes the inputs and we should do the attack
            //{
            //    //invoke event
            //    ResetCombo();
            //}

            return true;
        }
        else
        {
            return false;
        }
    }

    public ComboInput CurrentComboInput(int comboCount)
    {
        if (comboCount >= combo.Count) return null;
        return combo[comboCount].input;
    }
}
