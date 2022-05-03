using UnityEngine;

public partial class Data
{  
    public struct NewData
    {
        public int? Points { get; internal set; }
        public int? Level { get; internal set; }
        public int? SelectedTankIndex { get; internal set; }
    }
    public int Points => PlayerPrefs.GetInt(Keys.Points, 0);
    public int Level => PlayerPrefs.GetInt(Keys.Level, 0);
    public int[,] PointsSliderMinAndMaxValues { get; private set; } = new int[,]
    {
        { 0, 9900},
        {9900, 24560}
    };
    public int SelectedTankIndex => PlayerPrefs.GetInt(Keys.SelectedTankIndex, 0);
    
    [SerializeField] private TankProperties[] _availableTanks;
    public TankProperties[] AvailableTanks
    {
        get => _availableTanks;
    }


    public void OnDestroy()
    {
        PlayerPrefs.Save();
    }

    public void SetData(NewData newData)
    {
        if (newData.Points != null) PlayerPrefs.SetInt(Keys.Points, (int)newData.Points);
        if (newData.Level != null) PlayerPrefs.SetInt(Keys.Level, (int)newData.Level);
        if (newData.SelectedTankIndex != null) PlayerPrefs.SetInt(Keys.SelectedTankIndex, (int)newData.SelectedTankIndex);
    }
}
