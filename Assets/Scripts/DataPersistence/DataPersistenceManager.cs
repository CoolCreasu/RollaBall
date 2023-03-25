using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RollaBall.DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]
        [SerializeField] private string _fileName = "save.json";
        [SerializeField] private bool _useEncryption = false;

        private GameData _gameData = default;
        private List<IDataPersistence> _dataPersistenceObjects = default;
        private FileDataHandler _dataHandler = default;

        public static DataPersistenceManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one Data Persistence Manager in the scene.");
            }
        }

        private void Start()
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
            _dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        public void NewGame()
        {
            _gameData = new GameData();
        }

        public void LoadGame()
        {
            // Load any saved data from a file using the data handler
            _gameData = _dataHandler.Load();

            // if no data can be Loaded, initialize to a new game
            if (_gameData == null)
            {
                Debug.Log("No data was found. Initializing data to defaults.");
                NewGame();
            }

            // Push the loaded data to all other scripts that need it
            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(_gameData);
            }
        } 

        public void SaveGame()
        {
            // Pass the data to other scripts so they can update it
            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref _gameData);
            }

            // Save that data to a file using the data handler
            _dataHandler.Save(_gameData);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    }
}