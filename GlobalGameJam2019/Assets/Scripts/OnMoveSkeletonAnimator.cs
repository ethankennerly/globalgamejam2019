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

        [SerializeField]
        private bool m_OriginalFacesLeft;

        private Vector3 m_PreviousPosition;

        private void OnEnable()
        {
            m_PreviousPosition = transform.position;
        }

        private void Update()
        {
            Vector3 currentPosition = transform.position;
            if (currentPosition == m_PreviousPosition)
                return;

            float distance = Vector3.Distance(currentPosition, m_PreviousPosition);
            float deltaTime = Time.deltaTime;
            bool idle = true;
            if (deltaTime > 0f)
            {
                float speed = distance / deltaTime;
                if (speed > m_MaxIdleSpeed)
                    idle = false;
            }
            float angle = AngleBetweenPoints(m_PreviousPosition, currentPosition);
            bool front = angle < 0f || angle > 180f;
            string animation;
            if (idle)
                if (front)
                    animation = m_IdleFrontAnimation;
                else
                    animation = m_IdleBackAnimation;
            else
                if (front)
                    animation = m_MoveFrontAnimation;
                else
                    animation = m_MoveBackAnimation;
            if (m_Skeleton.AnimationName != animation)
                m_Skeleton.AnimationName = animation;

            bool left = (angle > 90f && angle < 270f) ||
                (angle < -90f && angle > -270f);
            bool flipX = m_OriginalFacesLeft ? !left : left;
            m_Skeleton.Skeleton.FlipX = flipX;

            m_PreviousPosition = transform.position;
        }

        /// <summary>
        /// <a href="https://answers.unity.com/questions/161138/deriving-and-angle-from-two-points.html">From two points</a>
        /// </summary>
        private static float AngleBetweenPoints(Vector2 p1, Vector2 p2)
        {
            return Mathf.Atan2(p2.y-p1.y, p2.x-p1.x) * Mathf.Rad2Deg;
        }
    }
}
