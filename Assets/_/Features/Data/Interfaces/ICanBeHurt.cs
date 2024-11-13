using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Runtime
{
    public interface ICanBeHurt
    {
        #region Main Methods

        void TakeDamage(int damage);

        #endregion
    }
}