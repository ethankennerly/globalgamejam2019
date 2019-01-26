using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class Containable : MonoBehaviour
    {
        [SerializeField]
        private bool m_GiveEnabled = true;
        public bool GiveEnabled
        {
            get { return m_GiveEnabled; }
            set { m_GiveEnabled = value; }
        }
    }
}
