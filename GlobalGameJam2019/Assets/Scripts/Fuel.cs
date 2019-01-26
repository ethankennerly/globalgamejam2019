using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class Fuel : MonoBehaviour
    {
        private float m_Quantity = 1f;
        public float quantity
        {
            get { return m_Quantity; }
            set { m_Quantity = value; }
        }

        public bool Add(float deltaQuantity)
        {
            m_Quantity += deltaQuantity;
            if (m_Quantity <= 0f)
                return false;

            return true;
        }
    }
}
