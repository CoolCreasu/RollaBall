using RollaBall.DataPersistence;
using RollaBall.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RollaBall
{
    public class PickUpsCollectedText : MonoBehaviour, IDataPersistence
    {
        private TextMeshProUGUI _pickUpsCollectedText = default;
        private int _pickUpsCollected = 0;

        private void Awake()
        {
            _pickUpsCollectedText = gameObject.GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            // subscribe to events
            GameEventsManager.Instance.OnPickUpCollected += OnPickUpCollected;
        }

        private void Update()
        {
            _pickUpsCollectedText.text = $"Collected {_pickUpsCollected} PickUps";
        }

        private void OnDestroy()
        {
            // unsubscribe from events
            GameEventsManager.Instance.OnPickUpCollected -= OnPickUpCollected;
        }

        private void OnPickUpCollected()
        {
            _pickUpsCollected++;

            if (_pickUpsCollected >= 12)
            {
                GameEventsManager.Instance.WinGame();
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
                GameEventsManager.Instance.WinGame();
            }
        }

        public void SaveData(ref GameData data)
        {
            // No data needs to be saved for this script
        }
    }
}