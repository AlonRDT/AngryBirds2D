using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(TrajectoryProjectionLogic))]
    public class SlingshotLogic : MonoBehaviour
    {
        [SerializeField] private GameObject m_BirdPrefab;
        [SerializeField] private LineRenderer[] m_LineRenderers;
        [SerializeField] private Transform[] m_StripTransforms;
        [SerializeField] private Transform m_Center;
        [SerializeField] private Transform m_IdlePosition;
        [SerializeField] private float m_MaxLength = 3.0f;
        [SerializeField] private float m_BirdPositionOffset = 0f;
        [SerializeField] private float m_BottomBoundry = 3.5f;
        [SerializeField] private float m_Force = 4;

        private Vector3 m_CurrentPosition;

        private bool m_IsMouseDown;

        private BirdLogic m_Bird;

        private TrajectoryProjectionLogic m_TrajectoryProjectionLogic;

        void Start()
        {
            m_TrajectoryProjectionLogic = GetComponent<TrajectoryProjectionLogic>();

            m_LineRenderers[0].positionCount = 2;
            m_LineRenderers[1].positionCount = 2;

            m_LineRenderers[0].SetPosition(0, m_StripTransforms[0].position);
            m_LineRenderers[1].SetPosition(0, m_StripTransforms[1].position);

            CreateBird();
        }

        public void CreateBird()
        {
            m_Bird = Instantiate(m_BirdPrefab).GetComponent<BirdLogic>();
            resetStrips();
        }

        void Update()
        {
            if (m_IsMouseDown == true)
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = 10;

                m_CurrentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                m_CurrentPosition = m_Center.position + Vector3.ClampMagnitude(m_CurrentPosition - m_Center.position, m_MaxLength);

                m_CurrentPosition = ClampBoundry(m_CurrentPosition);

                setStrips(m_CurrentPosition);

                Vector3 birdForce = (m_CurrentPosition - m_Center.position) * m_Force * -1;

                m_TrajectoryProjectionLogic.ProjectNewTrajectory(m_Bird, birdForce);
            }
        }

        private void OnMouseDown()
        {
            if (m_Bird != null)
            {
                m_IsMouseDown = true;
            }
        }

        private void OnMouseUp()
        {
            if (m_IsMouseDown == true)
            {
                m_IsMouseDown = false;
                shoot();
                m_CurrentPosition = m_IdlePosition.position;
                resetStrips();
                m_TrajectoryProjectionLogic.ClearVisuals();
            }
        }

        private void resetStrips()
        {
            m_CurrentPosition = m_IdlePosition.position;
            setStrips(m_IdlePosition.position);
        }

        private void setStrips(Vector3 position)
        {
            m_LineRenderers[0].SetPosition(1, position);
            m_LineRenderers[1].SetPosition(1, position);

            if (m_Bird != null)
            {
                Vector3 direction = position - m_Center.position;
                m_Bird.transform.position = position + direction.normalized * m_BirdPositionOffset;
                m_Bird.transform.right = -direction.normalized;
            }
        }

        private Vector3 ClampBoundry(Vector3 vector)
        {
            vector.y = Mathf.Clamp(vector.y, m_BottomBoundry, 1000);
            return vector;
        }

        private void shoot()
        {
            Vector3 birdForce = (m_CurrentPosition - m_Center.position) * m_Force * -1;
            m_Bird.BecomeProjectile(birdForce);
            m_Bird = null;
        }
    }
}
