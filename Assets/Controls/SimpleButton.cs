using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controls
{
    public class SimpleButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _textComponent;

        public void SetText(string text)
        {
            _textComponent.text = text;
        }
    }
}