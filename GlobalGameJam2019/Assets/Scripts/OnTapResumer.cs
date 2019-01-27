using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class OnTapResumer : MonoBehaviour
    {
        [SerializeField]
        private Clock m_Clock;

        [SerializeField]
        private bool m_Tapped;

        private void Update()
        {
            if (m_Tapped)
                return;

            if (!Input.GetMouseButtonDown(0))
                return;

            m_Tapped = true;

            m_Clock.paused = false;
        }
    }
}
