using UnityEngine;
using TMPro;

namespace FineGameDesign.FireFeeder
{
    public sealed class Clock : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text m_TimeText;

        [SerializeField]
        private bool m_Paused;
        public bool paused
        {
            get { return m_Paused; }
            set { m_Paused = value; }
        }

        [SerializeField]
        private float m_Seconds;
        private int m_PreviousSeconds = -1;
        private string m_PreviousFormat;

        public float deltaTime { get; private set; }

        private void Update()
        {
            deltaTime = Time.deltaTime;
            if (m_Paused)
            {
                deltaTime = 0f;
            }

            m_Seconds += deltaTime;
            m_TimeText.text = FormatTime(m_Seconds);
        }

        private string FormatTime(float seconds)
        {
            int wholeSeconds = (int)seconds;
            if (wholeSeconds < 0)
                wholeSeconds = 0;
            if (m_PreviousSeconds == wholeSeconds)
                return m_PreviousFormat;

            m_PreviousSeconds = wholeSeconds;

            const int kSecondsPerMinute = 60;
            int wholeMinutes = wholeSeconds / kSecondsPerMinute;
            int secondsPart = wholeSeconds % kSecondsPerMinute;
            string secondsFormat = secondsPart < 10 ?
                ":0" + secondsPart.ToString() :
                ":" + secondsPart.ToString();
            m_PreviousFormat = wholeMinutes.ToString() + secondsFormat;
            return m_PreviousFormat;
        }
    }
}
