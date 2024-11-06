using Data.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.Runtime
{
    public class ProjectileBehaviour : MonoBehaviour, IAmElement
    {
        #region Publics

        public IAmElement.Element spell => m_element;
        public IAmElement.Element m_element;

        #endregion
    }
}