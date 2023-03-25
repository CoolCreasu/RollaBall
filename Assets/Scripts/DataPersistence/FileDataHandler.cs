using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RollaBall.DataPersistence
{
    public class FileDataHandler
    {
        private string dataDirPath = string.Empty;
        private string dataFileName = string.Empty;

        public FileDataHandler(string dataDirPath, string dataFileName)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
        }

        public GameData Load()
        {
            // Use Path.Combine to account for different OS's having different path seperators
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            GameData loadedData = null;
            if (File.Exists(fullPath))
            {
                try
                {
                    // Load the serialized data from the file
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // Deserialize the data from Json back into the C# object
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error occured when trying to load data from file: {fullPath}\n{ex}");
                }
            }
            return loadedData;
        }

        public void Save(GameData data)
        {
            // Use Path.Combine to account for different OS's having different path seperators
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            try
            {
                // Create the directory the file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // Serialize the C# game data object into Json
                string dataToStore = JsonUtility.ToJson(data, true);

                // Write the serialized data to the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error occured when trying to save data to file: {fullPath}\n{ex}");
            }
        }
    }
}