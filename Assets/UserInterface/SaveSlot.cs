using System;
using System.Globalization;
using Modules.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class SaveSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleTextComponent;
        [SerializeField] private TextMeshProUGUI _saveDateTextComponent;
        [SerializeField] private GameObject _defaultState;
        [SerializeField] private GameObject _selectState;
        [SerializeField] private Button _delete;

        public SaveFileMetaData MetaData { get; private set; }

        public event Action OnSelect;
        public event Action OnDelete;

        public void Setup(SaveFileMetaData metaData)
        {
            MetaData = metaData;
            _titleTextComponent.text = metaData.Name;
            _saveDateTextComponent.text = metaData.SaveDate.ToString(CultureInfo.InvariantCulture);
            _delete.gameObject.SetActive(metaData.Type == SaveFileType.Manual);
        }

        public void SetDefault()
        {
            _defaultState.SetActive(true);
            _selectState.SetActive(false);
        }

        public void SetSelect()
        {
            _defaultState.SetActive(false);
            _selectState.SetActive(true);
        }

        public void OnClick()
        {
            OnSelect?.Invoke();
        }
        
        public void OnDeleteClick()
        {
            OnDelete?.Invoke();
        }
    }
}