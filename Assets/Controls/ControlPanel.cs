using System;
using Animations.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Controls
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Typewriter _typewriter;

        public event Action OnTypingComplete;
        public event Action OnNextClick;
        public event Action OnBackClick;

        public bool IsTyping => _typewriter.IsTyping;
        
        public void Init()
        {
            _typewriter.OnTypingComplete += () => OnTypingComplete?.Invoke();
        }
        
        public void DisableBackMove()
        {
            _backButton.interactable = false;
        }

        public void EnableBackMove()
        {
            _backButton.interactable = true;
        }

        public void TypeText(string text)
        {
            _typewriter.StartTyping(text);
        }

        public void ForceCompleteTyping()
        {
            _typewriter.ForceComplete();
        }

        public void OnNextButtonClick()
        {
            OnNextClick?.Invoke();
        }
        
        public void OnBackButtonClick()
        {
            OnBackClick?.Invoke();
        }
    }
}