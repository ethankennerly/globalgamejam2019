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

        [Header("Set max size.")]
        [SerializeField]
        private GameObject[] m_Pool;
        private int m_PoolIndex;

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
                bool hasPool = m_Pool != null && m_Pool.Length > 0;
                GameObject instance = !hasPool ? null : instance = m_Pool[m_PoolIndex];
                if (instance == null)
                {
                    instance = Instantiate(m_PrefabsToInstantiate[m_PrefabIndex], transform.position, transform.rotation);
                    if (hasPool)
                    {
                        m_Pool[m_PoolIndex] = instance;
                        ++m_PoolIndex;
                    }
                }
                else
                {
                    instance.SetActive(false);
                    instance.transform.position = transform.position;
                    instance.transform.rotation = transform.rotation;
                    instance.SetActive(true);
                }
                ++m_PrefabIndex;
                if (m_PrefabIndex >= m_PrefabsToInstantiate.Length)
                    m_PrefabIndex = 0;
            }
            m_PreviousPosition = transform.position;
        }
    }
}
