using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Animations.Text
{
    public class Typewriter : MonoBehaviour
    {
        [SerializeField] private float _typingSpeed = 0.05f;
        [SerializeField] private TextMeshProUGUI _textComponent;

        public event Action OnTypingComplete;

        private Coroutine _typingCoroutine;

        public bool IsTyping => _typingCoroutine != null;

        public void StartTyping(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                _textComponent.text = "";
                OnTypingComplete?.Invoke();
            }

            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);

            _typingCoroutine = StartCoroutine(TypeText(text));
        }

        private IEnumerator TypeText(string text)
        {
            _textComponent.text = text;
            _textComponent.ForceMeshUpdate();

            var totalVisibleCharacters = _textComponent.textInfo.characterCount;
            var visibleCount = 0;

            while (visibleCount <= totalVisibleCharacters)
            {
                _textComponent.maxVisibleCharacters = visibleCount;
                visibleCount++;
                yield return new WaitForSeconds(_typingSpeed);
            }

            _typingCoroutine = null;
            OnTypingComplete?.Invoke();
        }

        public void ForceComplete()
        {
            if (!IsTyping)
                return;
            
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
            
            _textComponent.maxVisibleCharacters = _textComponent.textInfo.characterCount;
            OnTypingComplete?.Invoke();
        }
    }
}