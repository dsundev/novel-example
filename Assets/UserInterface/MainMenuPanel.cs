using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Modules.SceneLoading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UserInterface
{
    public class MainMenuPanel : MonoBehaviour
    {
        [Inject] private SceneManager _sceneManager;
        [Inject] private SaveDataManager _saveDataManager;
        
        [SerializeField] private Button _resumeButton;

        public void Init()
        {
            _resumeButton.interactable = _saveDataManager.SaveFiles.Count > 0;
        }

        public void OnStartClick()
        {
            _saveDataManager.ResetSaveData();
            _sceneManager.LoadGame();
        }

        public void OnResumeClick()
        {
            var saveFiles = _saveDataManager.SaveFiles.OrderByDescending(x => x.MetaData.SaveDate).ToList();
            foreach (var saveFile in saveFiles)
            {
                if (!_saveDataManager.TryPreloadFrom(saveFile.MetaData.Id))
                    continue;
                
                _sceneManager.LoadGame(new List<IOperation> { _saveDataManager.ApplyPreloadedOperation });
                return;
            }
            
            _saveDataManager.ResetSaveData();
            _sceneManager.LoadGame();
        }

        public void OnExitClick()
        {
            Application.Quit();
        }
    }
}