using UnityEngine;
using UnityEngine.SceneManagement;

namespace FineGameDesign.FireFeeder
{
    public sealed class OnTapReloader : MonoBehaviour
    {
        [SerializeField]
        private float m_DisableTime = 1f;

        private float m_Time;

        private bool m_Tapped;

        private void Update()
        {
            if (m_Tapped)
                return;

            m_Time += Time.deltaTime;

            if (m_Time < m_DisableTime)
                return;

            if (!Input.GetMouseButtonDown(0))
                return;

            m_Tapped = true;

            SceneManager.LoadScene(0);
        }
    }
}
