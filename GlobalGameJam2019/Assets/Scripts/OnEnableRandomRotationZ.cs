using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class OnEnableRandomRotationZ : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Rotator;

        private void OnEnable()
        {
            SetRotationZ(m_Rotator, Random.value * 360f - 180f);
        }

        private static void SetRotationZ(Transform rotator, float degrees)
        {
            Vector3 angles = rotator.eulerAngles;
            angles.z = degrees;
            rotator.eulerAngles = angles;
        }
    }
}
