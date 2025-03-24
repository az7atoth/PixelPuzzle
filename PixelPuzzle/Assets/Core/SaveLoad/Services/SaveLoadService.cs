using UnityEngine;
using System.IO;

namespace PixelPuzzle
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string FILENAME = "/gamedata.json";

        public SaveData SaveData { get; private set; }

        public void ClearData()
        {
            Debug.Log("SaveLoad: making a new data");
            SaveData = new SaveData();
            SaveData.AppVersion = Application.version;
            Save();
        }

        public void Initialize()
        {
            var path = GetPath();

            var fileExist = File.Exists(path);

            if (fileExist)
            {
                Load();
            }
            else
            {
                ClearData();
            }
        }

        public void Load()
        {
            var path = GetPath();
            var text = File.ReadAllText(path);
            var data = JsonUtility.FromJson<SaveData>(text);

            if (data != null)
            {
                SaveData = data;
            }
            else
            {
                Debug.LogError("SaveLoad: saved data is null");
            }
        }

        public void Save()
        {
            var path = GetPath();
            var serialized = JsonUtility.ToJson(SaveData);
            File.WriteAllText(path, serialized);
        }

        private string GetPath()
        {
            return Application.persistentDataPath + FILENAME;
        }
    }

    public interface ISaveLoadService
    {
        public SaveData SaveData { get; }
        public void Initialize();
        void Save();
        void Load();
        void ClearData();
    }
}
