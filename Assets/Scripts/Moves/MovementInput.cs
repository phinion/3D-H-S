using UnityEngine;

namespace Moves
{
    public enum MovementType
    {
        idle = 0,
        forward = 1,
        right = 2,
        backward = 3,
        left = 4
    }

    [CreateAssetMenu(fileName = "MovementInput", menuName = "InputTypes/MovementInput")]
    public class MovementInput : BaseInputType
    {
        public MovementType requiredMovement;

        public override bool Matches(object instance)
        {
            if (instance is MovementInput movementInput)
            {
                return requiredMovement == movementInput.requiredMovement;
            }
            return false;
        }
    }
}
