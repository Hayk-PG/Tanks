using UnityEngine;

public partial class Data
{
    public int SelectedTankIndex => PlayerPrefs.GetInt(Keys.SelectedTankIndex, 0);
    public int SelectedAITankIndex { get; set; }


    [SerializeField] private TankProperties[] _availableTanks;
    [SerializeField] private AITankProperties[] _availableAiTanks;

    public TankProperties[] AvailableTanks => _availableTanks;
    public AITankProperties[] AvailableAITanks => _availableAiTanks;  
}
