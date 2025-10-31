using UnityEngine;
using UserInterface;
using Zenject;

namespace SceneManagement
{
    public class MainMenuSceneInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuPanel _mainMenuPanel;
        [SerializeField] private LoadPanelMenu _loadPanelMenu;
        
        public override void InstallBindings()
        {
            Container.Bind<MainMenuPanel>().To<MainMenuPanel>().FromInstance(_mainMenuPanel).AsSingle();
            Container.Bind<LoadPanelMenu>().To<LoadPanelMenu>().FromInstance(_loadPanelMenu).AsSingle();
        }
    }
}