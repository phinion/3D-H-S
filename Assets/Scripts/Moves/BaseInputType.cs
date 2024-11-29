using UnityEngine;

namespace Moves
{
    public abstract class BaseInputType : ScriptableObject
    {
        public abstract bool Matches(object instance);
    }
}