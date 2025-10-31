using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Modules.SaveLoad
{
    public class SaveFile<TMeta>
        where TMeta : class
    {
        public string Path { get; }
        public TMeta MetaData { get; set; }

        public SaveFile (string path, TMeta metaData)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));

            Path = path;
            MetaData = metaData;
        }

        public void LoadMetaData()
        {
            if (!File.Exists(Path))
                return;

            using (var fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
            {
                using (var br = new BinaryReader(fs))
                {
                    var metaLength = br.ReadInt32();
                    var metaBytes = br.ReadBytes(metaLength);
                    MetaData = JsonUtility.FromJson<TMeta>(Encoding.UTF8.GetString(metaBytes));
                }
            }
        }

        public bool TryLoadSaveData<T>(out T saveData) where T : class
        {
            saveData = default;
            
            if (!File.Exists(Path))
                return false;

            using (var fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
            {
                using (var br = new BinaryReader(fs))
                {
                    var metaLength = br.ReadInt32();
                    fs.Seek(metaLength, SeekOrigin.Current);
                    var saveBytesCount = fs.Length - fs.Position;
                    var saveBytes = br.ReadBytes((int)saveBytesCount);

                    try
                    {
                        saveData = JsonUtility.FromJson<T>(Encoding.UTF8.GetString(saveBytes));
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void Save<T>(T saveData) where T : class
        {
            var metaBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(MetaData));
            var saveBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(saveData));

            using (var fs = new FileStream(Path, FileMode.Create, FileAccess.Write))
            {
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(metaBytes.Length);
                    bw.Write(metaBytes);
                    bw.Write(saveBytes);
                }
            }
        }
    }
}