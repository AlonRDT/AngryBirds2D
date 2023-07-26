using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Singleton : MonoBehaviour
    {
        [SerializeField] private Transform m_ObstaclesParent;
        public Transform ObstaclesParent { get { return m_ObstaclesParent; } }

        [SerializeField] private SlingshotLogic m_SlingshotLogic;
        public SlingshotLogic Slingshot { get { return m_SlingshotLogic; } }

        public static Singleton Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}
