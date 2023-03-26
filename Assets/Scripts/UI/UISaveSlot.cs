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
                noDataContent.SetActive(true);
                hasDataContent.SetActive(false);
            }
            else // there is data for this profileId
            {
                noDataContent.SetActive(false);
                hasDataContent.SetActive(true);

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
        }
    }
}