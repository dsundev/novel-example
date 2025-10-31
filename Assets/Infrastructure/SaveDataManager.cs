using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Engine;
using Modules.SaveLoad;
using Modules.SceneLoading;
using Modules.ScreenshotCreation;
using UnityEngine;

namespace Infrastructure
{
    public class SaveDataManager
    {
        public class ApplyPreloadedSaveDataOperation : IOperation
        {
            private SaveDataManager _saveDataManager;
        
            public bool IsDone { get; private set; }

            public ApplyPreloadedSaveDataOperation(SaveDataManager saveDataManager)
            {
                IsDone = false;
                _saveDataManager = saveDataManager;
            }
        
            public async UniTask Run()
            {
                _saveDataManager.SaveData = _saveDataManager._preloadedData;
                _saveDataManager._preloadedData = null;
                
                await UniTask.Yield();
                IsDone = true;
            }
        }
        
        
        private const string AUTO_FILE_NAME = "Autosave";
        
        private SaveFileStorage<SaveData> _storage;
        private ScreenshotCreator _screenshotCreator;
        private SaveData _preloadedData;

        public SaveData SaveData { get; private set; }
        public IReadOnlyList<SaveFile<SaveFileMetaData>> SaveFiles => _storage.SaveFiles;
        public ApplyPreloadedSaveDataOperation ApplyPreloadedOperation => new ApplyPreloadedSaveDataOperation(this);

        public SaveDataManager()
        {
            var saveDirectoryPath = Path.Combine(Application.persistentDataPath, "saves");
            _storage = new SaveFileStorage<SaveData>(saveDirectoryPath, 1);
            _screenshotCreator = new ScreenshotCreator();
            SaveData = new SaveData();
        }

        public void Init()
        {
            _storage.Init();
            _storage.LoadMetaData();
        }
        
        public void SaveAuto(Camera camera)
        {
            var bytes = _screenshotCreator.CreateScreenshotBytes(camera);
            _storage.SaveAuto(AUTO_FILE_NAME, Convert.ToBase64String(bytes), SaveData);
        }
        
        public void SaveManual(string name, Camera camera)
        {
            var bytes = _screenshotCreator.CreateScreenshotBytes(camera);
            _storage.SaveManual(name, Convert.ToBase64String(bytes), SaveData);
        }
        
        public void Overwrite(string saveFileId, string name, Camera camera)
        {
            var bytes = _screenshotCreator.CreateScreenshotBytes(camera);
            _storage.Overwrite(saveFileId, name, Convert.ToBase64String(bytes), SaveData);
        }
        
        public void Remove(string saveFileId)
        {
            _storage.Remove(saveFileId);
        }

        public bool TryPreloadFrom(string saveFileId)
        {
            return _storage.TryLoadFrom(saveFileId, out _preloadedData);
        }

        public void ResetSaveData()
        {
            SaveData = new SaveData();
        }
    }
}