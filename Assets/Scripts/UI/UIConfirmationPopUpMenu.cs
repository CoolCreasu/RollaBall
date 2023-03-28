using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace RollaBall.UI
{
    public class UIConfirmationPopUpMenu : UIMenu
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _displayText = default;
        [SerializeField] private Button _confirmButton = default;
        [SerializeField] private Button _cancelButton = default;

        public void ActicateMenu(string displayText, UnityAction confirmAction, UnityAction cancelAction)
        {
            this.gameObject.SetActive(true);

            // set the display text
            _displayText.text = displayText;

            // Remove any existing listeners just to make sure there aren't any previous ones hanging around
            // note - this only removes listeners added through code
            _confirmButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();

            // Assign the onClick listeners
            _confirmButton.onClick.AddListener(() =>
            {
                DeactivateMenu();
                confirmAction();
            });
            _cancelButton.onClick.AddListener(() =>
            {
                DeactivateMenu();
                cancelAction();
            });
        }

        private void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }
    }
}