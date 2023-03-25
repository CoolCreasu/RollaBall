using RollaBall.Events;
using UnityEngine;

namespace RollaBall.UI
{
    public class UIWinText : MonoBehaviour
    {
        private bool _enabled = false;

        private void Start()
        {
            if (!_enabled) gameObject.SetActive(false);

            // subscribe to events
            // TODO - fix bug, incorrectly subscribing to event need to find the correct place for subscribing.
            GameEventsManager.Instance.OnWinGame += OnWinGame;
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