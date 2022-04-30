using UnityEngine;

public partial class Data
{
    public struct NewData
    {
        public int _points;
        public int _level;

        public NewData(int points, int level)
        {
            _points = points;
            _level = level;
        }
    }

    public void GetData()
    {
        Points = PlayerPrefs.GetInt(Keys.Points, 0);
        Level = PlayerPrefs.GetInt(Keys.Level, 0);
    }

    public void SaveData(NewData newData)
    {
        PlayerPrefs.SetInt(Keys.Points, newData._points);
        PlayerPrefs.SetInt(Keys.Level, newData._level);
        PlayerPrefs.Save();
    }
}
