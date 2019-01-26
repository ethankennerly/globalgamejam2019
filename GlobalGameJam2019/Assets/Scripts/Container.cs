using UnityEngine;

namespace FineGameDesign.FireFeeder
{
    public sealed class Container : MonoBehaviour
    {
        [SerializeField]
        private Containable[] m_Contents;
        public Containable[] contents
        {
            get { return m_Contents; }
        }

        [SerializeField]
        private Transform[] m_Parents;

        [SerializeField]
        private bool m_GiveEnabled;

        [SerializeField]
        private int m_AvailableIndex;

        public bool CanReceive(Containable item)
        {
            if (m_AvailableIndex >= m_Contents.Length)
                return false;

            if (item == null)
                return false;

            if (!item.GiveEnabled)
                return false;

            return true;
        }

        public bool TryReceive(Containable item)
        {
            if (!CanReceive(item))
                return false;

            int index = m_AvailableIndex;
            m_Contents[index] = item;
            item.GiveEnabled = m_GiveEnabled;
            ++m_AvailableIndex;

            int parentIndex = index >= m_Parents.Length ? m_Parents.Length - 1 : index;
            item.transform.SetParent(m_Parents[parentIndex], false);
            item.transform.localPosition = new Vector3(0f, 0f, 0f);

            return true;
        }

        public void TryReceiveContents(Container otherContainer)
        {
            if (!otherContainer.m_GiveEnabled)
                return;

            do
            {
                Containable otherItem = otherContainer.Peek();
                if (!CanReceive(otherItem))
                    break;

                otherItem = otherContainer.Pop();
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

        public Containable Peek()
        {
            if (m_AvailableIndex <= 0)
                return null;

            Containable item = m_Contents[m_AvailableIndex - 1];
            return item;
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
                return;
            }

            Containable containable = other.gameObject.GetComponent<Containable>();
            if (containable == null)
                return;

            TryReceive(containable);
        }
    }
}
