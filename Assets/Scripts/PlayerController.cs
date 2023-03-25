using RollaBall.DataPersistence;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RollaBall
{
    public class PlayerController : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private float _speed = 10.0f;

        private Rigidbody _rigidbody = default;
        private float _movementX = 0.0f;
        private float _movementY = 0.0f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnMove(InputValue movementValue)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();

            _movementX = movementVector.x;
            _movementY = movementVector.y;
        }

        private void FixedUpdate()
        {
            Vector3 movement = new Vector3(_movementX, 0.0f, _movementY);

            _rigidbody.AddForce(movement * _speed);
        }

        public void LoadData(GameData data)
        {
            transform.position = data.PlayerPosition;
        }

        public void SaveData(GameData data)
        {
            data.PlayerPosition = transform.position;
        }
    }
}