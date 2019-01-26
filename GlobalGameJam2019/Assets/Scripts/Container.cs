using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class Container : MonoBehaviour
    {
        [SerializeField]
        private Containable[] m_Contents;
        [SerializeField]
        private Transform[] m_Parents;
        private int m_AvailableIndex;

        public bool TryReceive(Containable item)
        {
            int index = m_AvailableIndex;
            if (index >= m_Contents.Length)
                return false;

            m_Contents[index] = item;
            ++m_AvailableIndex;

            if (index >= m_Parents.Length)
                return true;

            item.transform.SetParent(m_Parents[index], false);
            item.transform.localPosition = new Vector3(0f, 0f, 0f);

            return true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Containable containable = other.gameObject.GetComponent<Containable>();
            if (containable == null)
                return;

            TryReceive(containable);
        }
    }
}
