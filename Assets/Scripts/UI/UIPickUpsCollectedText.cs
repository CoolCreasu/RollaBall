using RollaBall.DataPersistence;
using RollaBall.Events;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RollaBall.UI
{
    public class UIPickUpsCollectedText : MonoBehaviour, IDataPersistence
    {
        private TextMeshProUGUI _pickUpsCollectedText = default;
        private int _pickUpsCollected = 0;

        private void Awake()
        {
            _pickUpsCollectedText = gameObject.GetComponent<TextMeshProUGUI>();

            // subscribe to events
            GameEventsManager.OnPickUpCollected += OnPickUpCollected;
        }

        private void Start()
        {
            // subscribe to events
            //GameEventsManager.OnPickUpCollected += OnPickUpCollected;
        }

        private void Update()
        {
            _pickUpsCollectedText.text = $"Collected {_pickUpsCollected} PickUps";
        }

        private void OnDestroy()
        {
            // unsubscribe from events
            GameEventsManager.OnPickUpCollected -= OnPickUpCollected;
        }

        private void OnPickUpCollected()
        {
            _pickUpsCollected++;

            if (_pickUpsCollected >= 12)
            {
                GameEventsManager.WinGame();
            }
        }

        public void LoadData(GameData data)
        {
            foreach (KeyValuePair<string, bool> pair in data.PickUpsCollected)
            {
                if (pair.Value)
                {
                    _pickUpsCollected++;
                }
            }

            if (_pickUpsCollected >= 12)
            {
                GameEventsManager.WinGame();
            }
        }

        public void SaveData(GameData data)
        {
            // No data needs to be saved for this script
        }
    }
}