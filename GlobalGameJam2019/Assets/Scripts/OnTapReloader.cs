using UnityEngine;
using UnityEngine.SceneManagement;

namespace FineGameDesign.FireFeeder
{
    public sealed class OnTapReloader : MonoBehaviour
    {
        private bool m_Tapped;

        private void Update()
        {
            if (m_Tapped)
                return;

            if (!Input.GetMouseButtonDown(0))
                return;

            m_Tapped = true;

            SceneManager.LoadScene(0);
        }
    }
}
