using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RollaBall.DataPersistence
{
    public class FileDataHandler
    {
        private string _dataDirPath = string.Empty;
        private string _dataFileName = string.Empty;
        private bool _useEncryption = false;
        private readonly string _encryptionCodeWord = "word";

        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            this._dataDirPath = dataDirPath;
            this._dataFileName = dataFileName;
            this._useEncryption = useEncryption;
        }

        public GameData Load()
        {
            // Use Path.Combine to account for different OS's having different path seperators
            string fullPath = Path.Combine(_dataDirPath, _dataFileName);
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

                    // optionally decrypt the data
                    if (_useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
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
            string fullPath = Path.Combine(_dataDirPath, _dataFileName);
            try
            {
                // Create the directory the file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // Serialize the C# game data object into Json
                string dataToStore = JsonUtility.ToJson(data, true);

                if (_useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

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

        // the below is a simple implementation of XOR encryption
        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char) (data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
            }
            return modifiedData;
        }
    }
}