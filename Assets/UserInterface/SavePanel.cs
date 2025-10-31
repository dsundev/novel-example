using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Infrastructure;
using Modules.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UserInterface
{
    public class SavePanel : MonoBehaviour
    {
        [Inject] private SaveDataManager _saveDataManager;

        [SerializeField] private SaveSlot _slotPrefab;
        [SerializeField] private Transform _slotsContainer;
        [SerializeField] private NewSaveSlot _newSaveSlot;
        [SerializeField] private GameObject _screenshotArea;
        [SerializeField] private GameObject _infoArea;
        [SerializeField] private TextMeshProUGUI _titleInfo;
        [SerializeField] private TextMeshProUGUI _saveDateInfo;
        [SerializeField] private Image _preview;
        [SerializeField] private Button _saveButton;

        private List<GameObject> _slots = new();
        private int _currentSelected;

        public void Show()
        {
            gameObject.SetActive(true);

            if (_slots.Count == 0)
                _slots.Add(_newSaveSlot.gameObject);
            
            UnselectAll();
            UpdateSaveSlots();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdateSaveSlots()
        {
            for (var i = 1; i < _slots.Count; i++)
            {
                _slots[i].SetActive(false);
            }

            var manualSaves = _saveDataManager.SaveFiles.Where(x => x.MetaData.Type == SaveFileType.Manual).OrderByDescending(x => x.MetaData.SaveDate).ToList();
            for (var i = 0; i < manualSaves.Count; i++)
            {
                var saveFile = manualSaves[i];
                
                SaveSlot slot;
                if (i < _slots.Count - 1)
                {
                    slot = _slots[i + 1].GetComponent<SaveSlot>();
                }
                else
                {
                    var index = i + 1;
                    slot = Instantiate(_slotPrefab, _slotsContainer, false);
                    slot.OnSelect += () => { SelectSlot(index); };
                    slot.OnDelete += () => { DeleteSlot(index); };
                    _slots.Add(slot.gameObject);
                }

                slot.gameObject.SetActive(true);
                slot.SetDefault();
                slot.Setup(saveFile.MetaData);
            }
        }

        private void DeleteSlot(int index)
        {
            if (index == 0)
                return;
            
            var slot = _slots[index].GetComponent<SaveSlot>();
            _saveDataManager.Remove(slot.MetaData.Id);
            
            UnselectAll();
            UpdateSaveSlots();
        }
        
        private void SelectSlot(int index)
        {
            if (index == 0)
            {
                _currentSelected = index;
                _slots[0].GetComponent<NewSaveSlot>().SetSelect();
                _screenshotArea.SetActive(false);
                _infoArea.SetActive(false);
                _saveButton.interactable = true;
            }
            else
            {
                _slots[0].GetComponent<NewSaveSlot>().SetDefault();
            }

            for (var i = 1; i < _slots.Count; i++)
            {
                var slot = _slots[i].GetComponent<SaveSlot>();
                if (i == index)
                {
                    _currentSelected = index;
                    slot.SetSelect();
                    _screenshotArea.SetActive(true);
                    _infoArea.SetActive(true);
                    _saveButton.interactable = true;
                    _titleInfo.text = slot.MetaData.Name;
                    _saveDateInfo.text = slot.MetaData.SaveDate.ToString(CultureInfo.InvariantCulture);
                    
                    if (!string.IsNullOrEmpty(slot.MetaData.Icon))
                    {
                        var texture = new Texture2D(2, 2);
                        texture.LoadImage(Convert.FromBase64String(slot.MetaData.Icon));
            
                        var sprite = Sprite.Create(
                            texture,
                            new Rect(0, 0, texture.width, texture.height),
                            new Vector2(0.5f, 0.5f)
                        );
                
                        _preview.sprite = sprite;
                    }
                }
                else
                {
                    slot.SetDefault();
                }
            }
        }

        private void UnselectAll()
        {
            _currentSelected = -1;
            
            _screenshotArea.SetActive(false);
            _infoArea.SetActive(false);
            _saveButton.interactable = false;
            
            _slots[0].GetComponent<NewSaveSlot>().SetDefault();
            
            for (var i = 1; i < _slots.Count; i++)
            {
                var slot = _slots[i].GetComponent<SaveSlot>();
                slot.SetDefault();
            }
        }

        public void OnClickNewSaveSlot()
        {
            SelectSlot(0);
        }

        public void OnSaveClick()
        {
            if (_currentSelected == 0)
            {
                var slot = _slots[0].GetComponent<NewSaveSlot>();
                if (!string.IsNullOrEmpty(slot.SaveName))
                {
                    _saveDataManager.SaveManual(slot.SaveName, Camera.main);
                }
            }
            else
            {
                var slot = _slots[_currentSelected].GetComponent<SaveSlot>();
                _saveDataManager.Overwrite(slot.MetaData.Id, slot.MetaData.Name, Camera.main);
            }
            
            UnselectAll();
            UpdateSaveSlots();
        }
    }
}