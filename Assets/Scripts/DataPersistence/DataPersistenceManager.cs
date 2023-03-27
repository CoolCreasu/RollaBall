using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RollaBall.DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("Debugging")]
        [SerializeField] private bool _disableDataPersistence = false;
        [SerializeField] private bool _initializeDataIfNull = false;
        [SerializeField] private bool _overrideSelectedProfileId = false;
        [SerializeField] private string _testSelectedProfileId = "test";

        [Header("File Storage Config")]
        [SerializeField] private string _fileName = "save.json";
        [SerializeField] private bool _useEncryption = false;

        private GameData _gameData;
        private List<IDataPersistence> _dataPersistenceObjects;
        private FileDataHandler _dataHandler;
        private string _selectedProfileId = string.Empty;

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

            if (_disableDataPersistence)
            {
                Debug.LogWarning("Data Persistence is currently disabled!");
            }

            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);

            this._selectedProfileId = _dataHandler.GetMostRecentlyUpdatedProfileId();
            if (_overrideSelectedProfileId)
            {
                this._selectedProfileId = _testSelectedProfileId;
                Debug.LogWarning("Overrode selected profile id with test id: " + _testSelectedProfileId);
            }
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

        public void ChangeSelectedProfileId(string newProfileId)
        {
            // Update the profile to use for saving and loading
            this._selectedProfileId = newProfileId;
            // Load the game, which will use that profile, updating our game data accordingly
            LoadGame();
        }

        public void NewGame()
        {
            _gameData = new GameData();
        }

        public void LoadGame()
        {
            // return right away if data persistence is disabled
            if (_disableDataPersistence)
            {
                return;
            }

            // Load any saved data from a file using the data handler
            _gameData = _dataHandler.Load(_selectedProfileId);

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
            // return right away if data persistence is disabled
            if (_disableDataPersistence)
            {
                return;
            }

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

            // Time stamp the data so we know when it was last saved
            _gameData.LastUpdated = System.DateTime.Now.ToBinary();

            // Save that data to a file using the data handler
            _dataHandler.Save(_gameData, _selectedProfileId);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            // FindObjectsOfType takes in an optional boolean to include inactive gameobjects
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public bool HasGameData()
        {
            return _gameData != null;
        }

        public Dictionary<string, GameData> GetAllProfilesGameData()
        {
            return _dataHandler.LoadAllProfiles();
        }
    }
}