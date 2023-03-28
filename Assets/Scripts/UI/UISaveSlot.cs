using RollaBall.DataPersistence;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RollaBall.UI
{
    public class UISaveSlot : MonoBehaviour
    {
        [Header("Profile")]
        [SerializeField] private string profileId = string.Empty;

        [Header("Content")]
        [SerializeField] private GameObject noDataContent;
        [SerializeField] private GameObject hasDataContent;
        [SerializeField] private TextMeshProUGUI percentageCompleteText;
        [SerializeField] private TextMeshProUGUI timeText;

        [Header("Clear Data Button")]
        [SerializeField] private Button clearButton;

        public bool hasData { get; private set; } = false;

        private Button saveSlotButton;

        private void Awake()
        {
            saveSlotButton = this.GetComponent<Button>();
        }

        public void SetData(GameData data)
        {
            // there's no data for this profileId
            if (data == null)
            {
                hasData = false;
                noDataContent.SetActive(true);
                hasDataContent.SetActive(false);
                clearButton.gameObject.SetActive(false);
            }
            else // there is data for this profileId
            {
                hasData = true;
                noDataContent.SetActive(false);
                hasDataContent.SetActive(true);
                clearButton.gameObject.SetActive(true);

                percentageCompleteText.text = $"{data.GetPercentageComplete()}% COMPLETE";
                timeText.text = $"{DateTime.FromBinary(data.LastUpdated)}";
            }
        }

        public string GetProfileId()
        {
            return this.profileId;
        }

        public void SetInteractable(bool interactable)
        {
            saveSlotButton.interactable = interactable;
            clearButton.interactable = interactable;
        }
    }
}