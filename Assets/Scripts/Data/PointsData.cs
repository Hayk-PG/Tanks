using UnityEngine;

public partial class Data
{
    [Header("Level")] [SerializeField] ScriptableObject_BadgesSprites _scriptableBadgeSprites;
    public int MaxLevel => _scriptableBadgeSprites.BadgesCount;
    public int[,] PointsSliderMinAndMaxValues { get; private set; } = new int[,]
    {
      { 0, 90000},
      {90000, 190000 },
      {190000, 315000 },
      {315000, 465000 },
      {465000, 640000 },
      {640000, 840000 },
      {840000, 1065000 },
      {1065000, 1315000 },
      {1315000, 1590000 },
      {1590000, 1890000 },
      {1890000, 2215000 },
      {2215000, 2565000 }
    };     
}
