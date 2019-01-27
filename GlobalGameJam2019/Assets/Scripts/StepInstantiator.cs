using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class StepInstantiator : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_PrefabsToInstantiate;

        [SerializeField]
        private float m_DistanceBetweenStep = 0.5f;
    }
}
