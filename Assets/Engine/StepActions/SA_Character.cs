using UnityEngine;
using Zenject;

namespace Engine.StepActions
{
    public class SA_Character : MonoBehaviour
    {
        [Inject] private CharacterManager _characterManager;

        [SerializeField] private int _charIndex;
        [SerializeField] private CharacterPosition _charPosition;
        [SerializeField] private int _charVariant;
        
        public void HideAll()
        {
            _characterManager.HideAll();
        }

        public void RunAction()
        {
            _characterManager.Show(_charIndex, _charPosition, _charVariant);
        }
    }
}