using System;
using System.Globalization;
using UnityEngine;

namespace Modules.SaveLoad
{
    [Serializable]
    public class SaveFileMetaData : ISerializationCallbackReceiver
    {
        [SerializeField] private string _saveDate;
        
        public string Id = string.Empty;
        public string Name = string.Empty;
        public string Icon = string.Empty;
        public DateTime SaveDate = DateTime.MinValue;
        public SaveFileType Type = SaveFileType.Auto;
        
        public void OnBeforeSerialize()
        {
            _saveDate = SaveDate.ToString(CultureInfo.InvariantCulture);
        }

        public void OnAfterDeserialize()
        {
            SaveDate = DateTime.Parse(_saveDate, CultureInfo.InvariantCulture);
        }
    }

    public enum SaveFileType
    {
        Auto,
        Manual
    }
}
