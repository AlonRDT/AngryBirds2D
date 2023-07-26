using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class BirdLogic : MonoBehaviour
    {
        private Rigidbody2D m_Rigidbody;
        private Collider2D m_Collider;

        private float m_DeathTimer = 0;
        private float m_DeathDelay = 7;

        private enum BirdState
        {
            Uninitialized,
            Idle,
            Projectile
        }
        private BirdState m_State;

        private void initialize()
        {
            if(m_State == BirdState.Uninitialized)
            {
                m_State = BirdState.Idle;

                m_Rigidbody = GetComponent<Rigidbody2D>();
                m_Rigidbody.isKinematic = true;
                m_Rigidbody.WakeUp();

                m_Collider = GetComponent<Collider2D>();
                m_Collider.enabled = false;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            initialize();
        }

        public void BecomeProjectile(Vector3 velocity)
        {
            initialize();

            m_Collider.enabled = true;
            m_Rigidbody.isKinematic = false;
            m_Rigidbody.velocity = velocity;

            m_State = BirdState.Projectile;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_State == BirdState.Projectile)
            {
                m_DeathTimer += Time.deltaTime;
                if (m_DeathTimer > m_DeathDelay || m_Rigidbody.velocity.magnitude < 0.1)
                {
                    Singleton.Instance.Slingshot.CreateBird();
                    Destroy(gameObject);
                }
            }
        }
    }
}