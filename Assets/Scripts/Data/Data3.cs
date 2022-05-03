using UnityEngine;

public partial class Data
{
    public int SelectedAITankIndex { get; set; }

    [SerializeField] private AITankProperties[] _availableAiTanks;
    public AITankProperties[] AvailableAITanks => _availableAiTanks;
}
