public class TournamentRoomData
{
    public string MasterOpponentName { get; private set; }
    public string SecondOpponentName { get; private set; }
    public string RoomName { get; private set; }


    public TournamentRoomData(string masterOpponentName, string secondOpponentName, string roomName)
    {
        MasterOpponentName = masterOpponentName;
        SecondOpponentName = secondOpponentName;
        RoomName = roomName;
    }
}
