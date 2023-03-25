using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RollaBall.DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("Debugging")]
        [SerializeField] private bool _initializeDataIfNull = false;

        [Header("File Storage Config")]
        [SerializeField] private string _fileName = "save.json";
        [SerializeField] private bool _useEncryption = false;

        private GameData _gameData;
        private List<IDataPersistence> _dataPersistenceObjects;
        private FileDataHandler _dataHandler;

        public static DataPersistenceManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        private void OnSceneUnloaded(Scene scene)
        {
            SaveGame();
        }

        public void NewGame()
        {
            _gameData = new GameData();
        }

        public void LoadGame()
        {
            // Load any saved data from a file using the data handler
            _gameData = _dataHandler.Load();

            // Start a new game if the data is null and we're configured to initialize data for debugging purposes
            if (_gameData == null && _initializeDataIfNull)
            {
                NewGame();
            }

            // If no data can be loaded, don't continue
            if (_gameData == null)
            {
                Debug.Log("No data was found. A new game needs to be started before data can be loaded.");
                return;
            }

            // Push the loaded data to all other scripts that need it
            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(_gameData);
            }
        } 

        public void SaveGame()
        {
            // If we don't have any data to save, log a warning here
            if (_gameData == null)
            {
                Debug.LogWarning("No data was found. A new game needs to be started before data can be saved.");
                return;
            }

            // Pass the data to other scripts so they can update it
            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(_gameData);
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

        public bool HasGameData()
        {
            return _gameData != null;
        }
    }
}