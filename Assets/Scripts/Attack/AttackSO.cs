using System.Collections.Generic;
using System.Linq;
using Moves;
using UnityEngine;

namespace Attack
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Moves/Attack")]
    public class AttackSO : ScriptableObject
    {
        public string Name;
        public string Anim;
        public bool MaintainMomentum;
        public float forwardImpulse = 0f;
        public bool useRootY = false;
        [SerializeField] private List<BaseInputType> requiredInputs;

        // Checks if all required inputs are met in the current inputs, ignoring extra inputs
        public bool ExistsIn(List<BaseInputType> currentInputs)
        {
            return requiredInputs.All(
                inputType => currentInputs.Exists(
                    input => input.Matches(inputType)));
        }
    }
}
