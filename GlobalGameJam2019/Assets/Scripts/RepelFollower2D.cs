using FineGameDesign.Utils;
using System;
using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class RepelFollower2D : MonoBehaviour
    {
        [SerializeField]
        private TapFollower2D m_Follower;

        [Header("Optional.")]
        [SerializeField]
        private Collider2D[] m_Repellants;

        [SerializeField]
        private float m_RepelDistance = 2f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Array.IndexOf(m_Repellants, other) < 0)
                return;

            Vector3 position = transform.position;
            Vector3 away = position - other.transform.position;
            away.Normalize();
            away *= m_RepelDistance;
            Vector3 retreatPoint = position + away;
            m_Follower.SetWorldDestination(retreatPoint.x, retreatPoint.y);
        }
    }
}

