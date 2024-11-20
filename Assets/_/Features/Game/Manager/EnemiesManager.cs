using Data.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class EnemiesManager : MonoBehaviour
    {
        #region Main Methods

        public void ActionShowEnemiesOnStart()
        {
            foreach(GameObject enemy in _enemies)
            {
                gameObject.SetActive(true);
            }
        }

        public void ActionDisableAllEnemies()
        {
            for(int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].activeSelf == false) continue;
                _enemies[i].TryGetComponent(out ICanBeImmobilized component);
                component.FreezePosition();
            }
        }

        public void ActionEnableAllEnemies()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].activeSelf == false) continue;
                _enemies[i].TryGetComponent(out ICanBeImmobilized component);
                component.UnFreezePosition();
            }
        }

        #endregion


        #region Privates & Protected

        [SerializeField] private List<GameObject> _enemies;

        #endregion
    }
}