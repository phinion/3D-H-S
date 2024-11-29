using System.Collections.Generic;
using Moves;

namespace Attack
{
    [System.Serializable]
    public class AttackCombo
    {
        public string name;
        public List<AttackSO> combo;
        public List<int> transitionTimes;

        public bool ContinueCombo(List<BaseInputType> currentInputs, int comboCount)
        {
            var currentComboInput = CurrentComboInput(comboCount);
            if (currentComboInput != null && currentComboInput.ExistsIn(currentInputs))
            {
                return true;
            }
            return false;
        }

        public AttackSO CurrentComboInput(int comboCount)
        {
            if (comboCount >= combo.Count) return null;
            return combo[comboCount];
        }
    }
}
