using Spine.Unity;
using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class ScaleAnimationBlender : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Scaler;

        [SerializeField]
        private SkeletonAnimation m_Skeleton;

        [SerializeField]
        private string m_BlendAnimation;

        [SerializeField]
        private int m_BlendTrackIndex = 1;

        [SerializeField]
        private bool m_BlendLoop = true;

        [SerializeField]
        private bool m_SubtractScale;

        [SerializeField]
        private float m_MinBlendWeight;

        private Spine.TrackEntry m_BlendTrack;

        private float m_BlendWeight;

        private void Update()
        {
            m_BlendWeight = m_Scaler.localScale.x;
            if (m_SubtractScale)
                m_BlendWeight = 1f - m_BlendWeight;
            m_BlendWeight = Mathf.Clamp01(m_BlendWeight);
            if (m_BlendWeight < m_MinBlendWeight)
                m_BlendWeight = 0f;

            if (m_BlendTrack == null && m_BlendWeight > 0f)
                m_BlendTrack = m_Skeleton.state.SetAnimation(m_BlendTrackIndex, m_BlendAnimation, m_BlendLoop);
            if (m_BlendTrack != null && m_BlendWeight <= 0f)
            {
                m_Skeleton.state.ClearTrack(m_BlendTrackIndex);
                m_BlendTrack = null;
            }
            if (m_BlendTrack == null)
                return;

            m_BlendTrack.Alpha = m_BlendWeight;
            m_BlendTrack.TrackEnd = float.MaxValue;
        }
    }
}
