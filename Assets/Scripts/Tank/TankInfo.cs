using System;
using UnityEngine;

public class TankInfo : MonoBehaviour
{
    [Serializable] public struct Items
    {
        public Item _item;
        public int _number;
    }

    [Header("Name")]
    [SerializeField] 
    private string _tankName;
    [Header("Stars")]
    [SerializeField] [Range(0,5)]
    private int _starsCount;
    [Header("Price")]
    [SerializeField] [Range(0,1000)]
    private int _getItNowPrice;
    [Header("Build")]
    [SerializeField] [Range(0, 24)]
    private int _initialBuildHours;
    [SerializeField] [Range(0, 60)]
    private int _initialBuildMinutes;
    [SerializeField] [Range(0, 60)]
    private int _initialBuildSeconds;
    [Header("Needed Level")]
    [SerializeField] [Range(0, 100)]
    private int _availableInLevel;  
    [SerializeField] private Items[] _requiredItems;
    

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
    public Items[] RequiredItems
    {
        get => _requiredItems;
        set => _requiredItems = value;
    }
    public int AvailableInLevel
    {
        get => _availableInLevel;
        set => _availableInLevel = value;
    }
}
