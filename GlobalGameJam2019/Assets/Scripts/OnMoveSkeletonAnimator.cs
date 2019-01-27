using Spine.Unity;
using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class OnMoveSkeletonAnimator : MonoBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_Skeleton;

        [SerializeField]
        private float m_MaxIdleSpeed = 0.01f;

        [SerializeField]
        private string m_IdleBackAnimation;

        [SerializeField]
        private string m_IdleFrontAnimation;

        [SerializeField]
        private string m_MoveBackAnimation;

        [SerializeField]
        private string m_MoveFrontAnimation;
    }
}
