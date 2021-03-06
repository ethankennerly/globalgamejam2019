using FineGameDesign.Utils;
using System;
using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class TapFollower2D : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Follower;

        [SerializeField]
        private float m_Speed = 1f;

        [Header("Optional.")]
        [SerializeField]
        private Transform m_Rotator;

        [Header("Optional.")]
        [SerializeField]
        private Collider2D[] m_Repellants;

        [SerializeField]
        private float m_RepelDistance = 4f;

        private bool m_HasDestination;
        private Vector2 m_Destination;
        private Vector2 m_Step;

        private ClickInputSystem m_Input = ClickInputSystem.instance;

        private Action<float, float> m_OnWorldHoldXY;

        private void OnEnable()
        {
            if (m_OnWorldHoldXY == null)
                m_OnWorldHoldXY = SetWorldDestination;

            m_Input.onWorldHoldXY -= m_OnWorldHoldXY;
            m_Input.onWorldHoldXY += m_OnWorldHoldXY;
        }

        private void OnDisable()
        {
            m_Input.onWorldHoldXY -= m_OnWorldHoldXY;
        }

        private void Update()
        {
            Step(m_Follower, m_Destination, m_Speed * Clock.instance.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Array.IndexOf(m_Repellants, other) < 0)
                return;

            Vector3 position = transform.position;
            Vector3 away = position - other.transform.position;
            away.Normalize();
            away *= m_RepelDistance;
            Vector3 retreatPoint = position + away;
            m_Destination = retreatPoint;            
        }

        public void SetWorldDestination(float worldX, float worldY)
        {
            m_HasDestination = true;
            m_Destination = new Vector3(worldX, worldY, 0f);

            Rotate(m_Rotator, m_Follower.position, m_Destination);
        }

        private void Step(Transform follower, Vector2 destination, float distance)
        {
            if (!m_HasDestination)
                return;

            Vector2 position2D = follower.position;
            m_Step = destination - position2D;
            if (m_Step.magnitude < distance)
            {
                follower.position = new Vector3(destination.x, destination.y,
                    follower.position.z);
                m_HasDestination = false;
                return;
            }

            m_Step.Normalize();
            m_Step *= distance;
            Vector2 nextPosition2D = position2D + m_Step;
            follower.position = new Vector3(nextPosition2D.x, nextPosition2D.y,
                follower.position.z);
        }

        private void Rotate(Transform rotator, Vector2 source, Vector2 destination)
        {
            if (rotator == null)
                return;

            float angle = AngleBetweenPoints(source, destination);
            Vector3 angles = rotator.eulerAngles;
            angles.z = angle;
            rotator.eulerAngles = angles;
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
