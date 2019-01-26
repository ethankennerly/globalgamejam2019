using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class Container : MonoBehaviour
    {
        [SerializeField]
        private Containable[] m_Contents;

        [SerializeField]
        private Transform[] m_Parents;

        [SerializeField]
        private bool m_GiveEnabled;

        private int m_AvailableIndex;

        public bool CanReceive()
        {
            return m_AvailableIndex < m_Contents.Length;
        }

        public bool TryReceive(Containable item)
        {
            if (!CanReceive())
                return false;

            int index = m_AvailableIndex;
            m_Contents[index] = item;
            ++m_AvailableIndex;

            if (index >= m_Parents.Length)
                return true;

            item.transform.SetParent(m_Parents[index], false);
            item.transform.localPosition = new Vector3(0f, 0f, 0f);

            return true;
        }

        public void TryReceiveContents(Container otherContainer)
        {
            if (!otherContainer.m_GiveEnabled)
                return;

            do
            {
                if (!CanReceive())
                    break;

                Containable otherItem = otherContainer.Pop();
                if (otherItem == null)
                    break;

                if (!TryReceive(otherItem))
                {
                    otherContainer.TryReceive(otherItem);
                    break;
                }
            }
            while (true);
        }

        public Containable Pop()
        {
            if (m_AvailableIndex <= 0)
                return null;

            --m_AvailableIndex;
            Containable item = m_Contents[m_AvailableIndex];
            return item;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Container otherContainer = other.gameObject.GetComponent<Container>();
            if (otherContainer != null)
            {
                TryReceiveContents(otherContainer);
                // otherContainer.TryReceiveContents(this);
                return;
            }

            Containable containable = other.gameObject.GetComponent<Containable>();
            if (containable == null)
                return;

            TryReceive(containable);
        }
    }
}
