using UnityEngine;

public partial class Data
{     
    public int Points => PlayerPrefs.GetInt(Keys.Points, 0);
    public int Level => PlayerPrefs.GetInt(Keys.Level, 0);
    public int[,] PointsSliderMinAndMaxValues { get; private set; } = new int[,]
    {
        { 0, 9900},
        {9900, 24560}
    };     
}
