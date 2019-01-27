using System;
using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class Chaser : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Follower;

        [SerializeField]
        private Transform m_Target;

        [SerializeField]
        private float m_Speed = 10f;

        [Header("Optional.")]
        [SerializeField]
        private Transform m_Rotator;

        [SerializeField]
        private Collider2D[] m_Repellants;

        [SerializeField]
        private Transform m_RepelledTarget;
        [SerializeField]
        private float m_RepelledDuration = 2f;
        private float m_RepelledTime;
        private bool m_Repelled;
        private bool m_InRepelTrigger;

        [SerializeField]
        private MonoBehaviour[] m_OnRepelDisablers;

        private bool m_HasDestination;
        private Vector2 m_Destination;
        private Vector2 m_Step;

        private void Update()
        {
            if (UpdateRepelled())
            {
                return;
            }

            Chase(m_Target);
        }

        private void Chase(Transform target)
        {
            if (target == null)
                return;

            SetWorldDestination(target.position.x, target.position.y);
            Step(m_Follower, m_Destination, m_Speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            StartRepelled(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (m_Repelled)
                return;

            StartRepelled(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            m_Repelled = false;
        }

        private void StartRepelled(Collider2D other)
        {
            if (Array.IndexOf(m_Repellants, other) < 0)
                return;

            foreach (MonoBehaviour disabler in m_OnRepelDisablers)
                disabler.enabled = false;

            m_Repelled = true;
            m_RepelledTime = m_RepelledDuration;
            Rotate(m_Rotator, m_Follower.position, other.transform.position);
            Vector2 destination = m_RepelledTarget.transform.position;
            SetWorldDestination(destination.x, destination.y);
        }

        private bool UpdateRepelled()
        {
            m_RepelledTime -= Time.deltaTime;
            if (m_RepelledTime >= 1f)
            {
                Step(m_Follower, m_Destination, m_Speed * Time.deltaTime);
                return true;
            }

            if (m_RepelledTime > 0f)
                return true;

            if (m_Repelled)
                return true;

            foreach (MonoBehaviour disabler in m_OnRepelDisablers)
                disabler.enabled = true;

            return false;
        }


        // TODO: Extract common methods from TapFollower2D and Chaser to Follower composition.

        private void SetWorldDestination(float worldX, float worldY)
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
