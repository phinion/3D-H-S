using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Moves/Attack")]
public class Attack : ScriptableObject
{
    public string Name;
    public string Anim;
    public bool MaintainMomentum;
    public float forwardImpulse = 0f;
    public bool useRootY = false;
    [SerializeField] private List<InputType> requiredInputs;

    // Checks if all required inputs are met in the current inputs, ignoring extra inputs
    public bool ExistsIn(List<InputType> currentInputs)
    {
        return requiredInputs.All(
            inputType => currentInputs.Exists(
                input => input.Matches(inputType)));
    }
}

[System.Serializable]
public class AttackCombo
{
    public string name;
    public List<Attack> combo;
    public List<int> transitionTimes;

    public bool ContinueCombo(List<InputType> currentInputs, int comboCount)
    {
        var currentComboInput = CurrentComboInput(comboCount);
        if (currentComboInput != null && currentComboInput.ExistsIn(currentInputs))
        {
            return true;
        }
        return false;
    }

    public Attack CurrentComboInput(int comboCount)
    {
        if (comboCount >= combo.Count) return null;
        return combo[comboCount];
    }
}
