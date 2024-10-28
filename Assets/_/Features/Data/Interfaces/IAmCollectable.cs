using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    public interface IAmCollectable
    {
        void Collect();

        void Disappear();
    }
}