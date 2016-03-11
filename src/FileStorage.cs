using System.Data;

namespace AutomationEngine
{
    public abstract class FileStorage
    {
        public abstract string StorageFileName { get; }
    }

    public abstract class FileStorage<T> : FileStorage where T : FileStorage, new()
    {
        private static readonly T TheInstance = LoadFromFile();

        public static T Instance
        {
            get { return TheInstance; }
        }

        private static T LoadFromFile()
        {
            string storageFileName = new T().StorageFileName;
            return XmlStorage.Load<T>(storageFileName) ?? new T();
        }

        public void Save()
        {
            XmlStorage.Save(StorageFileName, this);
        }
    }
}