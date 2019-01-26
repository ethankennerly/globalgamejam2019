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

        private bool m_HasDestination;
        private Vector2 m_Destination;
        private Vector2 m_Step;

        private ClickInputSystem m_Input = ClickInputSystem.instance;

        private Action<float, float> m_OnWorldXY;

        private void OnEnable()
        {
            if (m_OnWorldXY == null)
                m_OnWorldXY = SetWorldDestination;

            m_Input.onWorldXY -= m_OnWorldXY;
            m_Input.onWorldXY += m_OnWorldXY;
        }

        private void Update()
        {
            Step(m_Follower, m_Destination, m_Speed * Time.deltaTime);
        }

        private void SetWorldDestination(float worldX, float worldY)
        {
            m_HasDestination = true;
            m_Destination = new Vector3(worldX, worldY, 0f);
        }

        private void Step(Transform follower, Vector2 Destination, float distance)
        {
            if (!m_HasDestination)
                return;

            Vector2 position2D = follower.position;
            m_Step = Destination - position2D;
            if (m_Step.magnitude < distance)
            {
                follower.position = new Vector3(Destination.x, Destination.y,
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
    }
}
