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
        public float m_spacing = 1f;

        [Header("Prefab Settings")]
        [Tooltip("The m_plateform to instantiate in the grid.")]
        public GameObject m_plateform;
        
        #endregion

        #region Unity

        /// <summary>
        /// Generates the grid when the script is loaded or a value is changed in the Inspector.
        /// </summary>
        private void Start() => GenerateGrid();

        #endregion

        #region Methods

        public GameObject GetRandomPlateform() => _plateformList[Random.Range(0, _plateformList.Count)];

        #endregion

        #region Utils

        /// <summary>
        /// Generates a grid of prefabs based on the specified m_rows, m_columns, and m_spacing.
        /// </summary>
        private void GenerateGrid()
        {
            if (m_plateform == null) throw new System.Exception("Prefab is not assigned!");

            m_plateform.TryGetComponent(out MeshRenderer meshRenderer);
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

                    GameObject plateform = Instantiate(m_plateform, position, Quaternion.identity, transform);

                    if (row == m_rows / 2 && col == m_columns / 2) continue;
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
