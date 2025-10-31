using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Engine
{
    public class Episode : MonoBehaviour
    {
        [Inject] private SaveDataManager _saveDataManager;
        
        [SerializeField] private int _id;
        [SerializeField] private List<Step> _steps;

        [SerializeField] private UnityEvent _onStepsFinished;

        private int _current;

        public int Id => _id;
        public bool IsFirstStep => _current == 0;
        
        public void Run(int stepIndex)
        {
            _current = stepIndex;
            _saveDataManager.SaveData.EpisodeId = _id;

            if (TryRunStep(_current))
                return;

            RunStep(_steps.Count - 1);
        }
        
        public void NextStep()
        {
            _steps[_current].Release();
            
            if (!TryRunStep(++_current))
                _onStepsFinished?.Invoke();
        }
        
        public void BackStep()
        {
            _steps[_current].Release();

            var previous = _current - 1;
            if (!TryRunStep(previous))
            {
                Debug.LogError("The previous step could not be started.");
                return;
            }

            _current = previous;
        }

        private bool TryRunStep(int index)
        {
            if (index < 0 || index >= _steps.Count)
                return false;

            RunStep(index);
            return true;
        }

        private void RunStep(int index)
        {
            _saveDataManager.SaveData.StepIndex = index;
            
            _steps[index].Setup(this);
            _steps[index].Run();
            
            SaveAuto().Forget();
        }

        private async UniTaskVoid SaveAuto()
        {
            await UniTask.DelayFrame(2);
            _saveDataManager.SaveAuto(Camera.main);
        }
    }
}