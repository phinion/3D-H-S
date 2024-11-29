using UnityEngine;

namespace Moves
{
    [CreateAssetMenu(fileName = "InputType",menuName = "InputTypes/InputType")]
    public class InputType : BaseInputType
    {
        public string RequiredValue;

        public override bool Matches(object instance)
        {
            if (instance is InputType nameInputType)
            {
                return RequiredValue == nameInputType.RequiredValue;
            }
            return false;
        }
    }
}
