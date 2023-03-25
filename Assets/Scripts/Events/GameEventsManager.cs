using System;
using UnityEngine;

namespace RollaBall.Events
{
    public class GameEventsManager : MonoBehaviour
    {
        public static GameEventsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one Game Events Manager in the scene.");
            }
            Instance = this;
        }

        public event Action OnPickUpCollected;

        public void PickUpCollected()
        {
            if (OnPickUpCollected != null)
            {
                OnPickUpCollected();
            }
        }

        public event Action OnWinGame;

        public void WinGame()
        {
            Debug.Log("Called event");

            if (OnWinGame != null)
            {
                Debug.Log("Executing event");
                OnWinGame();
            }
        }
    }
}