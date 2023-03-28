using RollaBall.DataPersistence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RollaBall.UI
{
    public class UISaveSlotMenu : UIMenu
    {
        [Header("Menu Navigation")]
        [SerializeField] private UIMainMenu _mainMenu = default;

        [Header("Menu Buttons")]
        [SerializeField] private Button backButton = default;

        [Header("Confirmation Popup")]
        [SerializeField] private UIConfirmationPopUpMenu _confirmationPopUpMenu = default;

        private UISaveSlot[] _saveSlots = default;

        private bool isLoadingGame = false;

        private void Awake()
        {
            _saveSlots = gameObject.GetComponentsInChildren<UISaveSlot>();
        }

        public void OnSaveSlotClicked(UISaveSlot saveSlot)
        {
            // disable all menu buttons
            DisableMenuButtons();

            // case - Loading game
            if (isLoadingGame)
            {
                DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                SaveGameAndLoadScene();
            }
            // case - New game, but the save slot has data
            else if (saveSlot.hasData)
            {
                _confirmationPopUpMenu.ActicateMenu($"Starting a new game with this slot will override the currently saved data. Are you sure?",
                    // function to execute if we select 'yes'
                    () => {
                        DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                        DataPersistenceManager.Instance.NewGame();
                        SaveGameAndLoadScene();
                    }, 
                    // funcion to execute if we select 'cancel'
                    () => { 
                        // TODO - come back to this
                        this.ActivateMenu(isLoadingGame);
                    });
            }
            // case - New game, and the save slot has no data
            else
            {
                DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                DataPersistenceManager.Instance.NewGame();
                SaveGameAndLoadScene();
            }
        }

        private void SaveGameAndLoadScene()
        {
            // save the game anytime before loading a new scene
            DataPersistenceManager.Instance.SaveGame();
            // Load the scene
            SceneManager.LoadSceneAsync("MiniGame");
        }

        public void OnClearClicked(UISaveSlot saveSlot)
        {
            DisableMenuButtons();

            _confirmationPopUpMenu.ActicateMenu("Are you sure you want to delete this save data?", 
                // function to execute if we select 'yes'
                () => {
                    DataPersistenceManager.Instance.DeleteProfileData(saveSlot.GetProfileId());
                    ActivateMenu(isLoadingGame);
                },
                // function to execute if we select 'cancel'
                () => {
                    ActivateMenu(isLoadingGame);
                });
        }

        public void OnBackClicked()
        {
            _mainMenu.ActivateMenu();
            this.DeactivateMenu();
        }

        public void ActivateMenu(bool isLoadingGame)
        {
            this.gameObject.SetActive(true);

            // set mode
            this.isLoadingGame = isLoadingGame;

            // Load all the profiles that exist
            Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

            // Ensure the back button is enabled when we activate the menu
            backButton.interactable = true;

            // Loop through each save slot in the UI and set the content appropriately
            GameObject firstSelected = backButton.gameObject;
            foreach (UISaveSlot saveSlot in _saveSlots)
            {
                GameData profileData = null;
                profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
                saveSlot.SetData(profileData);
                if (profileData == null && isLoadingGame)
                {
                    saveSlot.SetInteractable(false);
                }
                else
                {
                    saveSlot.SetInteractable(true);
                    if (firstSelected.Equals(backButton.gameObject))
                    {
                        firstSelected = saveSlot.gameObject;
                    }
                }

                // set the first selected button
                Button firstSelectedButton = firstSelected.GetComponent<Button>();
                this.SetFirstSelected(firstSelectedButton);
            }
        }

        public void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }

        private void DisableMenuButtons()
        {
            foreach (UISaveSlot saveSlot in _saveSlots)
            {
                saveSlot.SetInteractable(false);
            }
            backButton.interactable = false;
        }
    }
}