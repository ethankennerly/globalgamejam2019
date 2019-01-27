using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class OnEnableShakeCamera : MonoBehaviour
    {
        [SerializeField]
        private float m_MaxDistanceFromCamera = 30f;

        [SerializeField]
        private float m_MaxShake = 3f;

        [SerializeField]
        private float m_Duration = 0.5f;

        private void OnEnable()
        {
            if (m_MaxDistanceFromCamera < 0f)
                return;

            Vector3 delta = Camera.main.transform.position - transform.position;
            float distance = delta.magnitude;
            float ratio = distance / m_MaxDistanceFromCamera;
            ratio = 1f - ratio;
            float shakeAmount = Mathf.Lerp(0f, m_MaxShake, ratio);
            if (shakeAmount <= 0f)
                return;

            CameraShake.instance.ShakeCamera(shakeAmount, m_Duration);
        }
    }
}
