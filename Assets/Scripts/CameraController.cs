using UnityEngine;

namespace RollaBall
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameObject _player = default;

        private Vector3 _offset = Vector3.zero;

        private void Start()
        {
            _offset = transform.position - _player.transform.position;
        }

        private void LateUpdate()
        {
            transform.position = _player.transform.position + _offset;
        }
    }
}