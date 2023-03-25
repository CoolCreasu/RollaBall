using RollaBall.Events;
using UnityEngine;

namespace RollaBall.UI
{
    public class WinText : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);

            // subscribe to events
            GameEventsManager.Instance.OnWinGame += OnWinGame;
        }

        private void OnDestroy()
        {
            // unsubscribe from events
            GameEventsManager.Instance.OnWinGame -= OnWinGame;
        }

        private void OnWinGame()
        {
            gameObject.SetActive(true);
        }
    }
}