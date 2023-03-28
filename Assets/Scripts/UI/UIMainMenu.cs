using RollaBall.DataPersistence;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

namespace RollaBall.UI
{
    public class UIMainMenu : UIMenu
    {
        [Header("Menu Navigation")]
        [SerializeField] private UISaveSlotMenu _saveSlotMenu = default;

        [Header("Menu Buttons")]
        [SerializeField] private Button _newGameButton = default;
        [SerializeField] private Button _continueGameButton = default;
        [SerializeField] private Button _loadGameButton = default;

        private void Start()
        {
            if (!DataPersistenceManager.Instance.HasGameData())
            {
                _continueGameButton.interactable = false;
                _loadGameButton.interactable = false;
            }
        }

        public void OnNewGameClicked()
        {
            _saveSlotMenu.ActivateMenu(false);
            this.DeactivateMenu();
        }

        public void OnLoadGameClicked()
        {
            _saveSlotMenu.ActivateMenu(true);
            this.DeactivateMenu();
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

        public void OnQuitGameClicked()
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
        }

        private void DisableMenuButtons()
        {
            _newGameButton.interactable = false;
            _continueGameButton.interactable = false;
        }

        public void ActivateMenu()
        {
            this.gameObject.SetActive(true);
        }

        public void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }
    }
}