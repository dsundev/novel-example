using Engine;
using Infrastructure;
using Modules.SaveLoad;
using Modules.SceneLoading;
using UnityEngine;
using Zenject;

namespace SceneManagement
{
    public class InitSceneStarter : MonoBehaviour
    {
        [Inject] private SceneManager _sceneManager;
        [Inject] private SaveDataManager _saveDataManager;
        
        private void Start()
        {
            // todo move to operations
            _saveDataManager.Init();
            // todo move to operations
            
            _sceneManager.LoadMainMenu();
        }
    }
}