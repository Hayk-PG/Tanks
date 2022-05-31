using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Data
{
    public int SelectedTankIndex => PlayerPrefs.GetInt(Keys.SelectedTankIndex, 0);
    public int SelectedAITankIndex { get; set; }

    [Header("Tanks")]
    [SerializeField] private TankProperties[] _availableTanks;
    [SerializeField] private AITankProperties[] _availableAiTanks;

    public TankProperties[] AvailableTanks => _availableTanks;
    public AITankProperties[] AvailableAITanks => _availableAiTanks;     
    

    private void CreateTanksReadOnlyData(string playfabId, Action<string, Dictionary<string, string>> updateUserDataRequest)
    {
        for (int i = 0; i < AvailableTanks.Length; i++)
        {
            string key = AvailableTanks[i]._tankName;
            string value = i == 0 ? PlayfabKeyAndValues.UnLocked : PlayfabKeyAndValues.Locked;
            updateUserDataRequest?.Invoke(playfabId, new Dictionary<string, string> { { key, value } });
        }
    }
}
