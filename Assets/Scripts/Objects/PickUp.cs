using RollaBall.DataPersistence;
using RollaBall.Events;
using System;
using UnityEngine;

namespace RollaBall
{
    public class PickUp : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private string _id = string.Empty;

        [ContextMenu("Generate guid for id")]
        private void GenerateGuid()
        {
            _id = Guid.NewGuid().ToString();
        }

        private bool _collected = false;

        private void Update()
        {
            transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && !_collected)
            {
                CollectPickUp();
            }
        }

        private void CollectPickUp()
        {
            _collected = true;
            gameObject.SetActive(false);
            GameEventsManager.PickUpCollected();
        }

        public void LoadData(GameData data)
        {
            data.PickUpsCollected.TryGetValue(_id, out _collected);
            if (_collected)
            {
                gameObject.SetActive(false);
            }
        }

        public void SaveData(GameData data)
        {
            if (data.PickUpsCollected.ContainsKey(_id))
            {
                data.PickUpsCollected.Remove(_id);
            }
            data.PickUpsCollected.Add(_id, _collected);
        }
    }
}