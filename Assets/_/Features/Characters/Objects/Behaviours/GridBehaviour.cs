using UnityEngine;

namespace Objects.Runtime
{
    /// <summary>
    /// Generates a grid of prefabs with specified m_rows, m_columns, and m_spacing.
    /// </summary>
    public class GridBehaviour : MonoBehaviour
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
        [Tooltip("The m_prefab to instantiate in the grid.")]
        public GameObject m_prefab;

        public GameObject m_centralObject { get; private set; }

        #endregion

        #region Unity

        /// <summary>
        /// Generates the grid when the script is loaded or a value is changed in the Inspector.
        /// </summary>
        private void Start() => GenerateGrid();

        #endregion

        #region Utils

        /// <summary>
        /// Generates a grid of prefabs based on the specified m_rows, m_columns, and m_spacing.
        /// </summary>
        private void GenerateGrid()
        {
            if (m_prefab == null) throw new System.Exception("Prefab is not assigned!");

            m_prefab.TryGetComponent(out MeshRenderer meshRenderer);
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

                    GameObject plateform = Instantiate(m_prefab, position, Quaternion.identity, transform);

                    if (row == m_rows / 2 && col == m_columns / 2) m_centralObject = plateform; 
                }
            }
        }

        #endregion
    }
}
