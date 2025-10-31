using Controls;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Engine.StepActions
{
    public class SA_ControlPanelTypingText : MonoBehaviour
    {
        [Inject] private ControlPanel _controlPanel;

        [TextArea(5,20)]
        [SerializeField] private string _text;

        [SerializeField] private UnityEvent _onTypingComplete;

        public void RunAction()
        {
            _controlPanel.OnTypingComplete += OnTypingComplete;
            _controlPanel.TypeText(_text);
        }

        private void OnTypingComplete()
        {
            _controlPanel.OnTypingComplete -= OnTypingComplete;
            _onTypingComplete?.Invoke();
        }
    }
}