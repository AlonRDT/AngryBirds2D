using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    [RequireComponent(typeof(LineRenderer))]
    public class TrajectoryProjectionLogic : MonoBehaviour
    {
        private class SimulatedGameObject
        {
            public Transform OriginalObject;
            public Transform CloneObject;
            public Rigidbody2D OriginalObjectRigidbody;
            public Rigidbody2D CloneObjectRigidbody;

            public SimulatedGameObject(Transform originalObject, Transform cloneObject)
            {
                OriginalObject = originalObject;
                OriginalObjectRigidbody = OriginalObject.GetComponent<Rigidbody2D>();
                CloneObject = cloneObject;
                CloneObjectRigidbody = CloneObject.GetComponent<Rigidbody2D>();
            }
        }
        private List<SimulatedGameObject> m_SimulatedObjects = null;

        private Scene m_SimulationScene;
        private PhysicsScene2D m_PhysicsScene;
        private LineRenderer m_LineRenderer;

        [SerializeField] private int m_MaxPhysicsFrameIterations = 100;
        [SerializeField] private GameObject m_SimulatedBirdPrefab;

        public void ProjectNewTrajectory(BirdLogic birdLogic, Vector3 velocity)
        {
            RefreshObstacleLocations();
            GameObject simulatedBird = Instantiate(m_SimulatedBirdPrefab, birdLogic.transform.position, birdLogic.transform.rotation);
            SceneManager.MoveGameObjectToScene(simulatedBird, m_SimulationScene);
            simulatedBird.GetComponent<Rigidbody2D>().velocity = velocity;

            if (m_LineRenderer == null)
            {
                m_LineRenderer = GetComponent<LineRenderer>();
            }

            m_LineRenderer.positionCount = m_MaxPhysicsFrameIterations;

            for (int i = 0; i < m_MaxPhysicsFrameIterations; i++)
            {
                m_PhysicsScene.Simulate(Time.fixedDeltaTime);
                m_LineRenderer.SetPosition(i, simulatedBird.transform.position);
            }

            Destroy(simulatedBird.gameObject);
        }

        public void ClearVisuals()
        {
            m_LineRenderer.positionCount = 0;
        }

        public void RefreshObstacleLocations()
        {
            if (m_SimulatedObjects == null)
            {
                m_SimulatedObjects = new List<SimulatedGameObject>();
                m_SimulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
                m_PhysicsScene = m_SimulationScene.GetPhysicsScene2D();

                foreach (Transform obstacle in Singleton.Instance.ObstaclesParent)
                {
                    Transform simulatedObstacle = SpawnObstacleIntoSimulation(obstacle);
                    m_SimulatedObjects.Add(new SimulatedGameObject(obstacle, simulatedObstacle));
                }
            }

            foreach (SimulatedGameObject simulatedGameObject in m_SimulatedObjects)
            {
                simulatedGameObject.CloneObject.position = simulatedGameObject.OriginalObject.position;
                simulatedGameObject.CloneObject.rotation = simulatedGameObject.OriginalObject.rotation;
                if (simulatedGameObject.CloneObjectRigidbody != null)
                {
                    simulatedGameObject.CloneObjectRigidbody.velocity = simulatedGameObject.OriginalObjectRigidbody.velocity;
                }
            }
        }

        private void OnDestroy()
        {
            SceneManager.UnloadSceneAsync(m_SimulationScene);
        }

        private Transform SpawnObstacleIntoSimulation(Transform obstacle)
        {
            GameObject simulatedObstacle = Instantiate(obstacle.gameObject, obstacle.position, obstacle.rotation);

            Renderer targetRenderer = simulatedObstacle.GetComponent<Renderer>();
            if (targetRenderer != null)
            {
                targetRenderer.enabled = false;
            }

            SceneManager.MoveGameObjectToScene(simulatedObstacle, m_SimulationScene);

            int layerSimulated = LayerMask.NameToLayer("Simulation");
            simulatedObstacle.layer = layerSimulated;

            return simulatedObstacle.transform;
        }
    }
}