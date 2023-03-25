using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RollaBall
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed = 10.0f;
        [SerializeField] private TextMeshProUGUI _countText = default;
        [SerializeField] private GameObject _winTextObject = default;

        private Rigidbody _rigidbody = default;
        private int _count = 0;
        private float _movementX = 0.0f;
        private float _movementY = 0.0f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            SetCountText();
            _winTextObject.SetActive(false);
        }

        private void OnMove(InputValue movementValue)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();

            _movementX = movementVector.x;
            _movementY = movementVector.y;
        }

        private void SetCountText()
        {
            _countText.text = $"Collected {_count} PickUps";
            if (_count >= 12)
            {
                _winTextObject.SetActive(true);
            }
        }

        private void FixedUpdate()
        {
            Vector3 movement = new Vector3(_movementX, 0.0f, _movementY);

            _rigidbody.AddForce(movement * _speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PickUp"))
            {
                other.gameObject.SetActive(false);
                _count = _count + 1;

                SetCountText();
            }
        }
    }
}