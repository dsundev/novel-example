using Controls;
using Engine;
using Infrastructure;
using UnityEngine;
using UserInterface;
using Zenject;

namespace SceneManagement
{
    public class GameSceneStarter : MonoBehaviour
    {
        [Inject] private SaveDataManager _saveDataManager;
        [Inject] private ChoicePanel _choicePanel;
        [Inject] private ControlPanel _controlPanel;
        [Inject] private BackgroundManager _backgroundManager;
        [Inject] private CharacterManager _characterManager;
        [Inject] private EpisodeManager _episodeManager;
        
        [Inject] private MenuPanel _menuPanel;
        [Inject] private SavePanel _savePanel;
        [Inject] private LoadPanelMenu _loadPanelMenu;

        private void Start()
        {
            _backgroundManager.Init();
            _controlPanel.Init();

            _characterManager.HideAll();
            _controlPanel.TypeText("");
            _choicePanel.Hide();
            
            _menuPanel.Hide();
            _savePanel.Hide();
            _loadPanelMenu.Hide();

            if (!_episodeManager.TryRun(_saveDataManager.SaveData.EpisodeId, _saveDataManager.SaveData.StepIndex))
                _episodeManager.TryRun(0, 0);
        }
    }
}