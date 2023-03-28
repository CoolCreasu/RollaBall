using System;
using UnityEngine;

namespace RollaBall.DataPersistence
{
    [Serializable]
    public class GameData
    {
        public long LastUpdated;
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

        public int GetPercentageComplete()
        {
            // figure out how many pickups we have collected
            int totalCollected = 0;
            foreach (bool collected in PickUpsCollected.Values)
            {
                if (collected)
                {
                    totalCollected++;
                }
            }

            // ensure we don't divide by 0 when calculating the percentage
            int percentageCompleted = -1;
            if (PickUpsCollected.Count != 0)
            {
                percentageCompleted = (totalCollected * 100 / PickUpsCollected.Count);
            }
            return percentageCompleted;
        }
    }
}