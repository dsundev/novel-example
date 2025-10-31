using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Modules.SaveLoad
{
    public class SaveFileStorage<TSave>
        where TSave : class, new()
    {
        private int _autoFilesCount;
        private bool _isInitialized;
        private string _saveDirectoryPath;
        private List<SaveFile<SaveFileMetaData>> _saveFiles;
        
        public IReadOnlyList<SaveFile<SaveFileMetaData>> SaveFiles => _saveFiles;

        public SaveFileStorage(string saveDirectoryPath, int autoFilesCount)
        {
            _autoFilesCount = autoFilesCount;
            _saveDirectoryPath = saveDirectoryPath;
            _saveFiles = new List<SaveFile<SaveFileMetaData>>();
        }

        public void Init()
        {
            if (_isInitialized)
                return;
            
            if (!Directory.Exists(_saveDirectoryPath))
                return;

            foreach (var filePath in Directory.GetFiles(_saveDirectoryPath))
            {
                _saveFiles.Add(new SaveFile<SaveFileMetaData>(filePath, new SaveFileMetaData()));
            }

            _isInitialized = true;
        }
        
        public void LoadMetaData()
        {
            foreach (var saveFile in _saveFiles)
            {
                saveFile.LoadMetaData();
            }
        }

        public bool TryLoadFrom(string saveFileId, out TSave data)
        {
            data = null;
            var saveFile = _saveFiles.FirstOrDefault(x => x.MetaData.Id == saveFileId);
            return saveFile != null && saveFile.TryLoadSaveData(out data);
        }

        public void Remove(string saveFileId)
        {
            var saveFile = _saveFiles.FirstOrDefault(x => x.MetaData.Id == saveFileId);
            if (saveFile == null)
                return;
            
            File.Delete(saveFile.Path);
            _saveFiles.Remove(saveFile);
        }

        public void SaveAuto(string name, string icon, TSave data)
        {
            if (_autoFilesCount < 1)
                return;
            
            if (!Directory.Exists(_saveDirectoryPath))
                Directory.CreateDirectory(_saveDirectoryPath);
            
            var autoSaveFiles = _saveFiles.Where(x => x.MetaData.Type == SaveFileType.Auto).OrderBy(x => x.MetaData.SaveDate).ToList();
            if (autoSaveFiles.Count < _autoFilesCount)
            {
                var guid = ShortGuid();
                var filePath = Path.Combine(_saveDirectoryPath, $"autosave_{guid}");
                var saveFile = new SaveFile<SaveFileMetaData>(filePath, new SaveFileMetaData
                {
                    Id = guid,
                    Name = name,
                    Icon = icon,
                    SaveDate = DateTime.Now,
                    Type = SaveFileType.Auto
                });
            
                saveFile.Save(data);
                _saveFiles.Add(saveFile);
            }
            else
            {
                var saveFile = autoSaveFiles[0];
                saveFile.MetaData.Name = name;
                saveFile.MetaData.Icon = icon;
                saveFile.MetaData.SaveDate = DateTime.Now;
                saveFile.Save(data);
            }
        }
        
        public void SaveManual(string name, string icon, TSave data)
        {
            if (!Directory.Exists(_saveDirectoryPath))
                Directory.CreateDirectory(_saveDirectoryPath);
            
            var guid = ShortGuid();
            var filePath = Path.Combine(_saveDirectoryPath, $"save_{guid}");
            var saveFile = new SaveFile<SaveFileMetaData>(filePath, new SaveFileMetaData
            {
                Id = guid,
                Name = name,
                Icon = icon,
                SaveDate = DateTime.Now,
                Type = SaveFileType.Manual
            });
            
            saveFile.Save(data);
            _saveFiles.Add(saveFile);
        }

        public void Overwrite(string saveFileId, string name, string icon, TSave data)
        {
            var saveFile = _saveFiles.First(x => x.MetaData.Id == saveFileId);
            saveFile.MetaData.Name = name;
            saveFile.MetaData.Icon = icon;
            saveFile.MetaData.SaveDate = DateTime.Now;
            saveFile.Save(data);
        }

        private string ShortGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}