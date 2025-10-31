using UnityEngine;
using Zenject;

namespace Engine.StepActions
{
    public class SA_Background : MonoBehaviour
    {
        [Inject] private BackgroundManager _backgroundManager;
        
        public void RunAction(int index)
        {
            _backgroundManager.Set(index);
        }
    }
}