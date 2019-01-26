using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class RefuelableLight : MonoBehaviour
    {
        [SerializeField]
        private Light m_LightWithCookie;

        [SerializeField]
        private float m_CurrentFuel = 1f;

        [SerializeField]
        private float m_MinFuel = 0f;

        [SerializeField]
        private float m_MaxFuel = 1f;

        [SerializeField]
        private float m_FuelPerSecond = -0.125f;

        [SerializeField]
        private float m_CookieSizePerFuel = 10f;

        private void Update()
        {
            UpdateFuel(Time.deltaTime);
            UpdateCookieSize(m_CurrentFuel);
        }

        private void UpdateFuel(float deltaTime)
        {
            m_CurrentFuel += m_FuelPerSecond * deltaTime;
            m_CurrentFuel = Mathf.Clamp(m_CurrentFuel, m_MinFuel, m_MaxFuel);
        }

        private void UpdateCookieSize(float fuel)
        {
            m_LightWithCookie.cookieSize = fuel * m_CookieSizePerFuel;
        }
    }
}
