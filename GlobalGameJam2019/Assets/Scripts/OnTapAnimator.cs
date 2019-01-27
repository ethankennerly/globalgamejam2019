using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class OnTapAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator m_TapAnimator;
        [SerializeField]
        private string m_TapAnimation;

        [SerializeField]
        private bool m_Tapped;

        private void Update()
        {
            if (m_Tapped)
                return;

            if (!Input.GetMouseButtonDown(0))
                return;

            m_Tapped = true;

            m_TapAnimator.Play(m_TapAnimation);
        }
    }
}
