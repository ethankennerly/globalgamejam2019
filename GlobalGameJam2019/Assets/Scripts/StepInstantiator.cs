using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class StepInstantiator : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_PrefabsToInstantiate;
        private int m_PrefabIndex;

        [SerializeField]
        private float m_DistanceBetweenStep = 4f;

        private Vector3 m_PreviousPosition;
        private float m_DistanceSinceLastStep;

        private void OnEnable()
        {
            m_PreviousPosition = transform.position;
        }

        private void Update()
        {
            m_DistanceSinceLastStep += Vector3.Distance(transform.position, m_PreviousPosition);
            if (m_DistanceSinceLastStep >= m_DistanceBetweenStep)
            {
                m_DistanceSinceLastStep -= m_DistanceBetweenStep;
                Instantiate(m_PrefabsToInstantiate[m_PrefabIndex], transform.position, Quaternion.identity);
                ++m_PrefabIndex;
                if (m_PrefabIndex >= m_PrefabsToInstantiate.Length)
                    m_PrefabIndex = 0;
            }
            m_PreviousPosition = transform.position;
        }
    }
}
