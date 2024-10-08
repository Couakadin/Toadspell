using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IGrappable
{
    #region Publics

    public IGrappable.Size m_grapSize => _enemySize;

    #endregion

    #region Privates

    [SerializeField]
    private IGrappable.Size _enemySize;

    #endregion
}
