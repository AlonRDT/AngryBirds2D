using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Instance.API.Movement
{
    [Serializable]
    public class LinearEnemyMovement : EnemyMovement
    {
        [SerializeField]
        private float m_Speed;
        /// <summ/ T d at which the enemy travels from one end of the path to the other.
        /// </summary>
        public float Speed => m_Speed;

        [SerializeField]
        private float m_PathLength;
        /// <summary>
        /// The entire length of the path the enemy will traverse horizontaly along the ground.
        /// </summary>
        public float PathLength => m_PathLength;

        public LinearEnemyMovement()
        {
            m_Speed = 0.2f;
            m_PathLength = 1f;
        }
    }
}
