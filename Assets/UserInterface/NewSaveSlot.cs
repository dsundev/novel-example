using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class NewSaveSlot : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_InputField _input;

        public string SaveName => _input.text;
        
        public void SetDefault()
        {
            _button.interactable = true;
            _title.gameObject.SetActive(true);
            _input.text = "";
            _input.gameObject.SetActive(false);
        }
        
        public void SetSelect()
        {
            _button.interactable = false;
            _title.gameObject.SetActive(false);
            _input.text = "";
            _input.gameObject.SetActive(true);
            _input.Select();
            _input.ActivateInputField();
        }
    }
}