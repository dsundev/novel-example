using UnityEngine;
using UserInterface;
using Zenject;

namespace SceneManagement
{
    public class MainMenuSceneStarter : MonoBehaviour
    {
        [Inject] private MainMenuPanel _mainMenuPanel;
        [Inject] private LoadPanelMenu _loadPanelMenu;

        private void Start()
        {
            _mainMenuPanel.Init();
            _loadPanelMenu.Hide();
        }
    }
}