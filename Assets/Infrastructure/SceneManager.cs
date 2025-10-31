using System.Collections.Generic;
using Modules.SceneLoading;

namespace Infrastructure
{
    public class SceneManager
    {
        private SceneLoader _sceneLoader;

        public SceneManager()
        {
            _sceneLoader = new SceneLoader();
        }
        
        public void LoadMainMenu(List<IOperation> operations = null)
        {
            _sceneLoader.Load("Loading", "MainMenu", operations).Forget();
        }

        public void LoadGame(List<IOperation> operations = null)
        {
            _sceneLoader.Load("Loading", "Game", operations).Forget();
        }
    }
}