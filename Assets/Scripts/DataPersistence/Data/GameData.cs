using System;
using UnityEngine;

namespace RollaBall.DataPersistence
{
    [Serializable]
    public class GameData
    {
        public Vector3 PlayerPosition;
        public Vector3 CameraPosition;
        public SerializableDictionary<string, bool> PickUpsCollected;

        // The values defined in this constructor will be the default values
        // the game starts with when there's no data to load
        public GameData()
        {
            PlayerPosition = new Vector3(0.0f, 0.5f, 0.0f);
            CameraPosition = new Vector3(0.0f, 10.0f, -10.0f);
            PickUpsCollected = new SerializableDictionary<string, bool>();
        }
    }
}