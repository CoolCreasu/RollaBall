using UnityEngine;

namespace RollaBall.DataPersistence
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);

        void SaveData(ref GameData data);
    }
}