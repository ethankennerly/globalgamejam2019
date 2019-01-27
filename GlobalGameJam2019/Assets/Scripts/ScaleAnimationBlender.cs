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
        private int m_BlendTrack = 1;

        [SerializeField]
        private bool m_SubtractScale;
    }
}
