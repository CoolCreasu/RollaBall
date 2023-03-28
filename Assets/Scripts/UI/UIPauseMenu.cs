using RollaBall.DataPersistence;
using RollaBall.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

namespace RollaBall.UI
{
    public class UIPauseMenu : UIMenu
    {
        [Header("Menu Buttons")]
        [SerializeField] private Button _resumeGameButton = default;
        [SerializeField] private Button _mainMenuButton = default;
        [SerializeField] private Button _quitGameButton = default;

        private void Awake()
        {
            GameEventsManager.OnPause += ActivateMenu;

            DeactvateMenu();
        }

        private void OnDestroy()
        {
            GameEventsManager.OnPause -= ActivateMenu;
        }

        public void OnResumeGameClicked()
        {
            DeactvateMenu();
        }

        public void OnMainMenuClicked()
        {
            DisableMenuButtons();
            // Save the game aytime before loading a new scene
            DataPersistenceManager.Instance.SaveGame();
            // Load the main menu scene
            SceneManager.LoadSceneAsync("MainMenu");
        }

        public void OnQuitGameClicked()
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
        }

        public void DisableMenuButtons()
        {
            _resumeGameButton.interactable = false;
            _mainMenuButton.interactable = false;
            _quitGameButton.interactable = false;
        }

        public void ActivateMenu()
        {
            this.gameObject.SetActive(true);
            Time.timeScale = 0.0f;


            _resumeGameButton.interactable = true;
            _mainMenuButton.interactable = true;
            _quitGameButton.interactable = true;
        }

        public void DeactvateMenu()
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
}