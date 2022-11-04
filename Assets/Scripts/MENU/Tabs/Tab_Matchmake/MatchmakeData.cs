public class MatchmakeData
{
    public string RoomName { get; set; }
    public string RoomPassword { get; set; }
    public bool IsRoomNameSet { get; set; }
    public bool IsRoomPasswordSet { get; set; }
    public bool IsRoomPublic { get; set; }
    public bool IsWindOn { get; set; }
    public int MapIndex { get; set; } = 0;
    public int Time { get; set; }
    public int Rounds { get; set; }
    public int DifficultyLevel { get; set; }
    public int Gold { get; set; }
    public int AmmoBox { get; set; }
    public int BasicBox { get; set; }
    public int WoodBox { get; set; }
    public int RewardChest { get; set; }
}
