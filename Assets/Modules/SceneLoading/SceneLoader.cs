using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace Modules.SceneLoading
{
    public class SceneLoader
    {
        private bool _isLoading;
        
        public async UniTaskVoid Load(string loadingSceneName, string targetSceneName, List<IOperation> operations = null)
        {
            if (_isLoading)
                return;
            
            _isLoading = true;
            
            await SceneManager.LoadSceneAsync(loadingSceneName).ToUniTask();
            await UniTask.Yield();

            if (operations?.Count > 0)
            {
                await UniTask.WhenAll(UniTask.Delay(1000, DelayType.Realtime), RunOperations(operations));
            }
            else
            {
                await (UniTask.Delay(1000, DelayType.Realtime));
            }

            var operation = SceneManager.LoadSceneAsync(targetSceneName);
            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                await UniTask.Yield();
            }

            operation.allowSceneActivation = true;

            while (!operation.isDone)
            {
                await UniTask.Yield();
            }

            _isLoading = false;
        }

        private async UniTask RunOperations(List<IOperation> operations)
        {
            foreach (var operation in operations)
            {
                await operation.Run();
            }
        }
    }
}