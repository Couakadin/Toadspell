using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    public interface ICanBeImmobilized
    {
        void FreezePosition();

        void UnFreezePosition();
    }
}