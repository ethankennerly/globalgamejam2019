using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class RefuelableLight : MonoBehaviour
    {
        [SerializeField]
        private Light m_LightWithCookie;

        [SerializeField]
        private Transform m_ScaleByFuel;

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

        [SerializeField]
        private Container m_Container;

        private void Update()
        {
            UpdateFuel(Time.deltaTime);
            UpdateCookieSize(m_CurrentFuel);
            UpdateScale(m_ScaleByFuel, m_CurrentFuel);
        }

        private void UpdateFuel(float deltaTime)
        {
            float previousFuel = GetFuel(m_Container);
            m_CurrentFuel = previousFuel + m_FuelPerSecond * deltaTime;
            m_CurrentFuel = Mathf.Clamp(m_CurrentFuel, m_MinFuel, m_MaxFuel);
            ChangeFuel(m_Container, previousFuel, m_CurrentFuel);
        }

        private void UpdateCookieSize(float fuel)
        {
            m_LightWithCookie.cookieSize = fuel * m_CookieSizePerFuel;
        }

        private void UpdateScale(Transform scaledTransform, float scale)
        {
            if (scaledTransform == null)
                return;

            scaledTransform.localScale = new Vector3(scale, scale, scale);
        }

        private float GetFuel(Container container)
        {
            float total = 0f;
            foreach (Containable item in container.contents)
            {
                if (item == null)
                    continue;

                Fuel fuel = item.GetComponent<Fuel>();
                if (fuel == null)
                    continue;
                
                total += fuel.quantity;
            }
            return total;
        }

        private void ChangeFuel(Container container, float previousFuel, float currentFuel)
        {
            float deltaFuel = currentFuel - previousFuel;
            Containable item = container.Peek();
            if (item == null)
                return;

            Fuel fuel = item.GetComponent<Fuel>();
            if (fuel == null)
                return;

            if (fuel.Add(deltaFuel))
                return;

            container.Pop();
            Destroy(fuel.gameObject);
        }
    }
}
