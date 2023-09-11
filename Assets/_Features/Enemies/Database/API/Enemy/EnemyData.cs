using Enemies.Instance.API.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies.Instance.API;
using UnityEditor;

namespace Enemies.Database.API.Enemy
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy")]
    public class EnemyData : ScriptableObject
    {
        private const string DEFAULT_ASSET_RESOURCES_DIRECTORY = "EnemyAssets";

        [Tooltip("The enemys unique name")]
        [SerializeField]
        private string m_EnemyName;
        /// <summary>
        /// The enemys unique name
        /// </summary>
        public string EnemyName => m_EnemyName;

        [Tooltip("The enemys model, includes sprite, animations and colliders")]
        [SerializeField]
        private GameObject m_EnemyModel;
        /// <summary>
        /// The enemys model, includes sprites, animations and colliders
        /// </summary>
        public GameObject EnemyModel => m_EnemyModel;

        [Tooltip("The enemys model scale")]
        [SerializeField]
        private Vector3 m_EnemyScale;
        /// <summary>
        /// The enemys model scale
        /// </summary>
        public Vector3 EnemyScale => m_EnemyScale;

        [Tooltip("The enemys sprite tint")]
        [SerializeField]
        private Color m_EnemySpriteTint;
        /// <summary>
        /// The enemys sprite tint
        /// </summary>
        public Color EnemySpriteTint => m_EnemySpriteTint;

        [Tooltip("The enemys movement behaviour")]
        [SerializeReference]
        private EnemyMovement m_EnemyMovement;
        /// <summary>
        /// The enemys sprite tint
        /// </summary>
        public EnemyMovement EnemyMovement => m_EnemyMovement;

        /// <summary>
        /// Load all enemy data to memory from deafault resources directory: <see cref="DEFAULT_ASSET_RESOURCES_DIRECTORY"/>
        /// </summary>
        /// <returns>An array of enemy data</returns>
        public static EnemyData[] LoadAllEnemies()
        {
            return Resources.LoadAll<EnemyData>(path: DEFAULT_ASSET_RESOURCES_DIRECTORY);
        }

        /// <summary>
        /// Create a game object instance of the enemy
        /// </summary>
        /// <returns>Enemy game object</returns>
        public GameObject SpawnInstance(Vector3 worldPosition)
        {
            GameObject output = new GameObject(m_EnemyName);
            output.transform.position = worldPosition;

            GameObject model = Instantiate(m_EnemyModel);
            model.transform.parent.SetParent(output.transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = m_EnemyScale;

            foreach (SpriteRenderer spriteRenderer in output.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.color = m_EnemySpriteTint;
            }

            EnemyLogic enemyLogic = output.AddComponent<EnemyLogic>();

            return output;
        }

        public void SetEnemyMovement(EnemyMovement enemyMovement)
        {
            m_EnemyMovement = enemyMovement;

            AssetDatabase.SaveAssets();
        }
    }
}
