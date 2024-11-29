using UnityEngine;

namespace Moves
{
    public enum AttackType { light = 0, heavy = 1 }
    
    [CreateAssetMenu(fileName = "AttackInput",menuName = "InputTypes/AttackInput")]
     public class AttackInput : BaseInputType
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
}
