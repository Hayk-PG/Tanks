using System.Collections.Generic;

public class TournamentObjectData 
{
    public static string ObjectName { get; private set; } = "Status";

    public static string KeyTournamentStatus { get; private set; } = "TournamentStatus";
    public static string KeyTournamentName { get; private set; } = "TournamentName";
    public static string KeyRoomStatus { get; private set; } = "RoomStatus";
    public static string KeyRoomName { get; private set; } = "RoomName";
    public static string KeyGameStatus { get; private set; } = "GameStatus";

    public static string ValueActive { get; private set; } = "Active";
    public static string ValuePassive { get; private set; } = "Passive";
    public static string ValueNotSet { get; private set; } = "N/A";


    /// <summary>
    /// 1:KeyTournamentStatus, 2:KeyTournamentName, 3:KeyRoomStatus, 4:KeyRoomName, 5:KeyGameStatus
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ObjectData(object[] values)
    {
        return new Dictionary<string, string>
        {
            {KeyTournamentStatus, (string)values[0]},
            {KeyTournamentName, (string)values[1]},
            {KeyRoomStatus, (string)values[2]},
            {KeyRoomName, (string)values[3]},
            {KeyGameStatus, (string)values[4]}
        };
    }
}
