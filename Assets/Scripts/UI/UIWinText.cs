using RollaBall.Events;
using UnityEngine;

namespace RollaBall.UI
{
    public class UIWinText : MonoBehaviour
    {
        private bool _enabled = false;

        private void OnEnable()
        {
            // subscribe to events
            // Important to subscribe in OnEnable and make the execution order put this script after the Event Manager
            GameEventsManager.Instance.OnWinGame += OnWinGame;
        }

        private void Start()
        {
            if (!_enabled) gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            // unsubscribe from events
            GameEventsManager.Instance.OnWinGame -= OnWinGame;
        }

        private void OnWinGame()
        {
            _enabled = true;
            gameObject.SetActive(true);
        }
    }
}