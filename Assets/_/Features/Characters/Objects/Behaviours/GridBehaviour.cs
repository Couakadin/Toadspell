using Data.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.Runtime
{
    /// <summary>
    /// Generates a grid of prefabs with specified m_rows, m_columns, and m_spacing.
    /// </summary>
    public class GridBehaviour : MonoBehaviour, IGrid
    {
        #region Publics

        [Header("Grid Settings")]
        [Tooltip("The number of m_rows in the grid.")]
        public int m_rows = 5;
        [Tooltip("The number of m_columns in the grid.")]
        public int m_columns = 5;
        [Tooltip("The m_spacing between prefabs (set to 0 to have prefabs touching each other).")]
        public float m_spacing = 0f;

        [Header("Boss Settings")]
        [Tooltip("The offset of the boss in Y.")]
        public float m_offsetHeightBoss;

        [Header("Prefab Settings")]
        [Tooltip("The platform prefab to instantiate in the grid.")]
        public GameObject m_platform;
        [Tooltip("The boss prefab to instantiate in the grid.")]
        public GameObject m_boss;

        public GameObject m_centralPlatform { get; set; }

        #endregion

        #region Unity

        /// <summary>
        /// Generates the grid when the script is loaded or a value is changed in the Inspector.
        /// </summary>
        private void Awake() => GenerateGrid();

        #endregion

        #region Methods

        public GameObject GetRandomPlatform() => _plateformList[Random.Range(0, _plateformList.Count)];

        #endregion

        #region Utils

        /// <summary>
        /// Generates a grid of prefabs based on the specified m_rows, m_columns, and m_spacing.
        /// </summary>
        private void GenerateGrid()
        {
            if (m_platform == null) throw new System.Exception("Prefab is not assigned!");

            m_platform.TryGetComponent(out MeshRenderer meshRenderer);
            if (meshRenderer == null) throw new System.Exception("Prefab does not have a MeshRenderer!");

            float width = meshRenderer.bounds.size.x;
            float depth = meshRenderer.bounds.size.z;

            for (int row = 0; row < m_rows; row++)
            {
                for (int col = 0; col < m_columns; col++)
                {
                    Vector3 position = new Vector3(
                        col * (width + m_spacing),
                        0,
                        row * (depth + m_spacing)
                    );

                    GameObject plateform = Instantiate(m_platform, position, Quaternion.identity, transform);

                    if (row == m_rows / 2 && col == m_columns / 2)
                    {
                        m_centralPlatform = plateform;
                        m_boss.transform.position = m_centralPlatform.transform.position + new Vector3(0, m_offsetHeightBoss,0);
                        continue;
                    }
                    _plateformList.Add(plateform);
                }
            }
        }

        #endregion

        #region Privates

        private List<GameObject> _plateformList = new();

        #endregion
    }
}
