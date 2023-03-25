using RollaBall.DataPersistence;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RollaBall.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("Menu Buttons")]
        [SerializeField] private Button _newGameButton = default;
        [SerializeField] private Button _continueGameButton = default;

        private void Start()
        {
            if (!DataPersistenceManager.Instance.HasGameData())
            {
                _continueGameButton.interactable = false;
            }
        }

        public void OnNewGameClicked()
        {
            DisableMenuButtons();
            // Create a new game - which will initialize our game data
            DataPersistenceManager.Instance.NewGame();
            // Load the gameplay scene which will in turn save the game because of
            // OnSceneUnloaded() in the DataPersistenceManager
            SceneManager.LoadSceneAsync("MiniGame");
        }

        public void OnContinueGameClicked()
        {
            DisableMenuButtons();
            // Save the game aytime before loading a new scene
            DataPersistenceManager.Instance.SaveGame();
            // Load the next scene - which will in turn load the game because of
            // OnSceneLoaded() in the DataPersistenceManager
            SceneManager.LoadSceneAsync("MiniGame");
        }

        private void DisableMenuButtons()
        {
            _newGameButton.interactable = false;
            _continueGameButton.interactable = false;
        }
    }
}