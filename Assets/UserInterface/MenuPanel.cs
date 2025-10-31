using UnityEngine;

namespace UserInterface
{
    public class MenuPanel : MonoBehaviour
    {
        public void Show()
        {
            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}