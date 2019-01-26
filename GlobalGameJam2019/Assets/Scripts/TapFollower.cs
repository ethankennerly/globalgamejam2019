using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class TapFollower : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Follower;

        [SerializeField]
        private float m_Speed = 1f;
    }
}
