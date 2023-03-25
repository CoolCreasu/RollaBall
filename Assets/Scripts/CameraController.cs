using RollaBall.DataPersistence;
using UnityEngine;

namespace RollaBall
{
    public class CameraController : MonoBehaviour, IDataPersistence
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

        public void LoadData(GameData data)
        {
            transform.position = data.CameraPosition;
        }

        public void SaveData(GameData data)
        {
            data.CameraPosition = transform.position;
        }
    }
}