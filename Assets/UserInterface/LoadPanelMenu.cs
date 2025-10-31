using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Engine;
using Infrastructure;
using Modules.SaveLoad;
using Modules.SceneLoading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UserInterface
{
    public class LoadPanelMenu : MonoBehaviour
    {
        [Inject] private SceneManager _sceneManager;
        [Inject] private SaveDataManager _saveDataManager;

        [SerializeField] private SaveSlot _slotPrefab;
        [SerializeField] private Transform _slotsContainer;
        [SerializeField] private GameObject _screenshotArea;
        [SerializeField] private GameObject _infoArea;
        [SerializeField] private TMP_Text _titleInfo;
        [SerializeField] private TMP_Text _saveDateInfo;
        [SerializeField] private Image _preview;
        [SerializeField] private Button _loadButton;
        
        private List<SaveSlot> _slots = new();
        private int _currentSelected;
        
        public void Show()
        {
            gameObject.SetActive(true);
            
            UnselectAll();
            UpdateSaveSlots();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void SelectSlot(int index)
        {
            for (var i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                if (i == index)
                {
                    _currentSelected = index;
                    slot.SetSelect();
                    _screenshotArea.SetActive(true);
                    _infoArea.SetActive(true);
                    _loadButton.interactable = true;
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
        
        private void DeleteSlot(int index)
        {
            var slot = _slots[index];
            _saveDataManager.Remove(slot.MetaData.Id);
            
            UnselectAll();
            UpdateSaveSlots();
        }
        
        private void UpdateSaveSlots()
        {
            foreach (var slot in _slots)
            {
                slot.gameObject.SetActive(false);
            }

            var saveFiles = _saveDataManager.SaveFiles.OrderByDescending(x => x.MetaData.SaveDate).ToList();
            for (var i = 0; i < saveFiles.Count; i++)
            {
                var saveFile = saveFiles[i];
                
                SaveSlot slot;
                if (i < _slots.Count)
                {
                    slot = _slots[i];
                }
                else
                {
                    var index = i;
                    slot = Instantiate(_slotPrefab, _slotsContainer, false);
                    slot.OnSelect += () => { SelectSlot(index); };
                    slot.OnDelete += () => { DeleteSlot(index); };
                    _slots.Add(slot);
                }

                slot.gameObject.SetActive(true);
                slot.SetDefault();
                slot.Setup(saveFile.MetaData);
            }
        }
        
        private void UnselectAll()
        {
            _currentSelected = -1;
            
            _screenshotArea.SetActive(false);
            _infoArea.SetActive(false);
            _loadButton.interactable = false;

            foreach (var slot in _slots)
            {
                slot.SetDefault();
            }
        }
        
        public void OnLoadClick()
        {
            var slot = _slots[_currentSelected];
            if (_saveDataManager.TryPreloadFrom(slot.MetaData.Id))
            {
                _sceneManager.LoadGame(new List<IOperation> { _saveDataManager.ApplyPreloadedOperation });
            }
            else
            {
                UnselectAll();
                UpdateSaveSlots();
            }
        }
    }
}