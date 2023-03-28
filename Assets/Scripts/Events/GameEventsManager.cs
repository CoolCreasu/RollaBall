using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace RollaBall.Events
{
    public class GameEventsManager : MonoBehaviour
    {
        public static GameEventsManager Instance { get; private set; }

        [SerializeField] UnityEngine.InputSystem.InputActionAsset _inputActions = default;

        private InputAction _actionCancel = default;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one Game Events Manager in the scene.");
            }
            Instance = this;

            _actionCancel = _inputActions.FindAction("UI/Cancel");
            _actionCancel.Enable();
        }

        private void Update()
        {
            if (_actionCancel.WasPressedThisFrame())
            {
                Pause();
            }
        }

        public static event Action OnPause = delegate { };
        public static void Pause()
        {
            OnPause();
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