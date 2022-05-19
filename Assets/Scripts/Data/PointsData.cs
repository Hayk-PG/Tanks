using UnityEngine;

public partial class Data
{     
    public int Points => PlayerPrefs.GetInt(Keys.Points, 0);
    public int Level => PlayerPrefs.GetInt(Keys.Level, 0);
    public int[,] PointsSliderMinAndMaxValues { get; private set; } = new int[,]
    {
      { 0, 9900},
      {9900, 24560},
      {24560, 39060 },
      {39060, 56560 },
      {56560, 76625 },
      {76625, 102475 },
      {102475, 137475 },
      {137475, 196075 },
      {196075, 291075 },
      {291075, 416075 },
      {416075, 591075 },
      {591075, 826075 }
    };     
}
