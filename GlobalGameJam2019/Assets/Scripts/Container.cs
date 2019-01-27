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

        [Header("Takes even if give is not enabled.")]
        [SerializeField]
        private bool m_IsThief;

        [SerializeField]
        private int m_AvailableIndex;

        [SerializeField]
        private Container m_AdditionalReceiver;

        [SerializeField]
        private GameObject m_ActivatedEachReceive;

        public bool CanReceive(Containable item)
        {
            if (m_AvailableIndex >= m_Contents.Length)
                return false;

            if (item == null)
                return false;

            if (!item.GiveEnabled && !m_IsThief)
                return false;

            return true;
        }

        private void Replace(Containable item, int index)
        {
            m_Contents[index] = item;
            item.GiveEnabled = m_GiveEnabled;

            int parentIndex = index >= m_Parents.Length ? m_Parents.Length - 1 : index;
            item.transform.SetParent(m_Parents[parentIndex], false);
            item.transform.localPosition = new Vector3(0f, 0f, 0f);
        }

        public bool TryReceive(Containable item)
        {
            if (!CanReceive(item))
                return false;

            int index = m_AvailableIndex;
            ++m_AvailableIndex;
            Replace(item, index);
            if (m_ActivatedEachReceive != null)
            {
                m_ActivatedEachReceive.SetActive(false);
                m_ActivatedEachReceive.SetActive(true);
            }

            return true;
        }

        public void TryReceiveContents(Container otherContainer)
        {
            if (!otherContainer.m_GiveEnabled && !m_IsThief)
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

            if (m_IsThief)
                TrySwapForMostFuel(otherContainer);
        }

        private void TrySwapForMostFuel(Container otherContainer)
        {
            Containable item = Peek();
            if (item == null)
                return;

            Fuel fuel = item.GetComponent<Fuel>();
            Containable best = null;
            float mostFuel = fuel == null ? 0f : fuel.quantity;
            float maxFuel = 1f;
            int bestIndex = -1;

            for (int index = 0, numContents = otherContainer.contents.Length; index < numContents; ++index)
            {
                Containable otherItem = otherContainer.contents[index];
                if (mostFuel >= maxFuel)
                    break;

                if (otherItem == null)
                    continue;

                Fuel otherFuel = otherItem.GetComponent<Fuel>();
                if (otherFuel == null)
                    continue;
                
                if (otherFuel.quantity > mostFuel)
                {
                    mostFuel = otherFuel.quantity;
                    best = otherItem;
                    bestIndex = index;
                }
            }

            if (best == null)
                return;

            item = Pop();
            if (!TryReceive(best))
                return;

            otherContainer.Replace(item, bestIndex);
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
                Container receiver = m_AdditionalReceiver;
                if (receiver != null)
                    receiver.TryReceiveContents(otherContainer);
                return;
            }

            Containable containable = other.gameObject.GetComponent<Containable>();
            if (containable == null)
                return;

            TryReceive(containable);
        }
    }
}
