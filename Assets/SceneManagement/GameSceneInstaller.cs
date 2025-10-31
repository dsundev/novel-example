using Controls;
using Engine;
using UnityEngine;
using UserInterface;
using Zenject;

namespace SceneManagement
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private ControlPanel _controlPanel;
        [SerializeField] private ChoicePanel _choicePanel;
        [SerializeField] private BackgroundManager _backgroundManager;
        [SerializeField] private CharacterManager _characterManager;
        [SerializeField] private EpisodeManager _episodeManager;
        
        [SerializeField] private MenuPanel _menuPanel;
        [SerializeField] private SavePanel _savePanel;
        [SerializeField] private LoadPanelMenu _loadPanelMenu;
        
        public override void InstallBindings()
        {
            Container.Bind<ControlPanel>().To<ControlPanel>().FromInstance(_controlPanel).AsSingle();
            Container.Bind<ChoicePanel>().To<ChoicePanel>().FromInstance(_choicePanel).AsSingle();
            Container.Bind<BackgroundManager>().To<BackgroundManager>().FromInstance(_backgroundManager).AsSingle();
            Container.Bind<CharacterManager>().To<CharacterManager>().FromInstance(_characterManager).AsSingle();
            Container.Bind<EpisodeManager>().To<EpisodeManager>().FromInstance(_episodeManager).AsSingle();
            
            Container.Bind<MenuPanel>().To<MenuPanel>().FromInstance(_menuPanel).AsSingle();
            Container.Bind<SavePanel>().To<SavePanel>().FromInstance(_savePanel).AsSingle();
            Container.Bind<LoadPanelMenu>().To<LoadPanelMenu>().FromInstance(_loadPanelMenu).AsSingle();
        }
    }
}