using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Instance.API.Movement
{
    [Serializable]
    public class CircularEnemyMovement : EnemyMovement
    {
        [SerializeField]
        private float m_MaxSpeed;
        /// <summary>
        /// peak speed reached at middle of path
        /// </summary>
        public float MaxSpeed => m_MaxSpeed;

        [SerializeField]
        private float m_WaitAtPathEndTime;
        /// <summary>
        /// enemy moves from one end of a linear line to another and waits an amount of seconds at each end before traveling again.
        /// </summary>
        public float WaitAtPathEndTime => m_WaitAtPathEndTime;

        [SerializeField]
        private float m_PathLength;
        /// <summary>
        /// The entire length of the path the enemy will traverse horizontaly along the ground.
        /// </summary>
        public float PathLength => m_PathLength;
    }
}
