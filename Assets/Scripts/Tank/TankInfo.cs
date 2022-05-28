using UnityEngine;

public class TankInfo : MonoBehaviour
{
    [SerializeField] private string _tankName;
    [SerializeField] private int _starsCount;
    [SerializeField] private int _getItNowPrice;
    [SerializeField] private int _initialBuildHours;
    [SerializeField] private int _initialBuildMinutes;
    [SerializeField] private int _initialBuildSeconds;
    [SerializeField] private int _availableInLevel;

    public string TankName
    {
        get => _tankName;
        set => _tankName = value;
    }
    public int StarsCount
    {
        get => _starsCount;
        set => _starsCount = value;
    }
    public int GetItNowPrice
    {
        get => _getItNowPrice;
        set => _getItNowPrice = value;
    }
    public int InitialBuildHours
    {
        get => _initialBuildHours;
        set => _initialBuildHours = value;
    }
    public int InitialBuildMinutes
    {
        get => _initialBuildMinutes;
        set => _initialBuildMinutes = value;
    }
    public int InitialBuildSeconds
    {
        get => _initialBuildSeconds;
        set => _initialBuildSeconds = value;
    }
    public int AvailableInLevel
    {
        get => _availableInLevel;
        set => _availableInLevel = value;
    }
}
