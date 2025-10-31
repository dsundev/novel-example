using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controls
{
    public class ChoicePanel : MonoBehaviour
    {
        [SerializeField] private List<SimpleButton> _buttons;

        public event Action<int> OnSelect; 

        public bool IsVisible => gameObject.activeSelf;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Setup(List<string> configurations)
        {
            foreach (var button in _buttons)
            {
                button.gameObject.SetActive(false);
            }

            for (var i = 0; i < configurations.Count; i++)
            {
                if (i >= _buttons.Count)
                    break;

                var button = _buttons[i];
                if (button == null)
                    continue;
                
                button.gameObject.SetActive(true);
                button.SetText(configurations[i]);
            }
        }

        public void OnButtonClick(int index)
        {
            OnSelect?.Invoke(index);
        }
    }
}