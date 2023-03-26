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

            // update the selected profile id to be used for data persistence
            DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

            if (!isLoadingGame)
            {
                // create a new game - which will initialize our data to a clean slate
                DataPersistenceManager.Instance.NewGame();
            }

            // Load the scene - which will in turn save the game because of OnSceneUnloaded() in the DataPersistenceManager
            SceneManager.LoadSceneAsync("MiniGame");
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

            GameObject firstSelected = backButton.gameObject;
            // Loop through each save slot in the UI and set the content appropriately
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
                StartCoroutine(this.SetFirstSelected(firstSelected));
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