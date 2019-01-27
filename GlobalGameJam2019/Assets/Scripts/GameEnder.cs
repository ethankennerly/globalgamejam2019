using UnityEngine;
using UnityEngine.SceneManagement;

namespace FineGameDesign.FireFeeder
{
    public sealed class GameEnder : MonoBehaviour
    {
        [SerializeField]
        private Collider2D m_ColliderEndsGame;

        [SerializeField]
        private Clock m_Clock;

        [SerializeField]
        private bool m_GameEnded;

        [SerializeField]
        private Animator m_EndAnimator;
        [SerializeField]
        private string m_EndAnimation;

        private void Update()
        {
            if (!m_GameEnded)
                return;

            if (!Input.GetMouseButtonDown(0))
                return;

            SceneManager.LoadScene(0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other != m_ColliderEndsGame)
                return;

            m_GameEnded = true;
            m_Clock.paused = true;
            m_EndAnimator.Play(m_EndAnimation);
        }
    }
}
