using UnityEngine;

public enum AttackType { light = 0, heavy = 1 }

public enum MovementType
{
    idle = 0,
    forward = 1,
    right = 2,
    backward = 3,
    left = 4
}

// Generic base class for input types
public abstract class InputType : ScriptableObject
{
    public abstract bool Matches(object instance);
}

// Specific implementation for attack input
[CreateAssetMenu(menuName = "InputTypes/AttackInput")]
public class AttackInput : InputType
{
    public AttackType requiredAttackType;

    public override bool Matches(object instance)
    {
        // Cast to AttackType and check for a match
        if (instance is AttackInput attackInput)
        {
            return requiredAttackType == attackInput.requiredAttackType;
        }
        return false;
    }
}

// Movement input type with MovementType
[CreateAssetMenu(menuName = "InputTypes/MovementInput")]
public class MovementInput : InputType
{
    public MovementType requiredMovement;

    public override bool Matches(object instance)
    {
        // Cast to MovementType and check for a match
        if (instance is MovementInput movementInput)
        {
            return requiredMovement == movementInput.requiredMovement;
        }
        return false;
    }
}

// Bool-based input type (used for dash, dodge, etc.)
[CreateAssetMenu(menuName = "InputTypes/BoolInputType")]
public class BoolInputType : InputType
{
    public bool requiredValue;

    public override bool Matches(object instance)
    {
        // Cast to bool and check for a match
        if (instance is bool boolValue)
        {
            return requiredValue == boolValue;
        }
        return false;
    }
}
