using Enemies.Instance.API.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Instance.API
{
    public class EnemyLogic : MonoBehaviour
    {
        private string m_EnemyName;
        /// <summary>
        /// Enemy's unique name, initialized from enemy data when sapwned
        /// </summary>
        public string EnemyName
        {
            get => m_EnemyName;
            private set => m_EnemyName = value;
        }

        /// <summary>
        /// Enemy routine movement behaviour.
        /// </summary>
        private EnemyMovement m_Movement;

        /// <summary>
        /// Sets data on start
        /// </summary>
        /// <param name="enemyName"><see cref="EnemyName"/></param>
        public void Initialize(string enemyName, EnemyMovement enemyMovement)
        {
            m_EnemyName = enemyName;
            m_Movement = enemyMovement;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
