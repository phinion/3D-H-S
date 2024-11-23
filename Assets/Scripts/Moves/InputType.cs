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

public abstract class InputType : ScriptableObject
{
    public abstract bool Matches(object instance);
}

[CreateAssetMenu(fileName = "AttackInput",menuName = "InputTypes/AttackInput")]
public class AttackInput : InputType
{
    public AttackType requiredAttackType;

    public override bool Matches(object instance)
    {
        if (instance is AttackInput attackInput)
        {
            return requiredAttackType == attackInput.requiredAttackType;
        }
        return false;
    }
}

[CreateAssetMenu(fileName = "MovementInput",menuName = "InputTypes/MovementInput")]
public class MovementInput : InputType
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

[CreateAssetMenu(fileName = "BoolInputType",menuName = "InputTypes/BoolInputType")]
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

[CreateAssetMenu(fileName = "NameInputType",menuName = "InputTypes/NameInputType")]
public class NameInputType : InputType
{
    public string requiredValue;

    public override bool Matches(object instance)
    {
        // Cast to bool and check for a match
        if (instance is NameInputType nameInputType)
        {
            return requiredValue == nameInputType.requiredValue;
        }
        return false;
    }
}