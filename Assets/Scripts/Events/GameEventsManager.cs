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

        public static event Action OnPickUpCollected = delegate { };
        public static void PickUpCollected()
        {
            OnPickUpCollected();
        }

        public static event Action OnWinGame = delegate { };
        public static void WinGame()
        {
            OnWinGame();
        }
    }
}