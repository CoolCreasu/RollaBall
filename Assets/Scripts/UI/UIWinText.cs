using RollaBall.Events;
using UnityEngine;

namespace RollaBall.UI
{
    public class UIWinText : MonoBehaviour
    {
        private bool _enabled = false;

        private void Awake()
        {
            // subscribe to events
            GameEventsManager.OnWinGame += OnWinGame;
        }

        private void Start()
        {
            if (!_enabled) gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            // unsubscribe from events
            GameEventsManager.OnWinGame -= OnWinGame;
        }

        private void OnWinGame()
        {
            _enabled = true;
            gameObject.SetActive(true);
        }
    }
}