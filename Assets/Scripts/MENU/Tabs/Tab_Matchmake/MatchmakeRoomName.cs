using System;

public class MatchmakeRoomName : MatchmakeRoomInfo, IMatchmakeData
{
    public override string TextResultOffline()
    {
        return "\n";
    }

    public override string TextResultOnline()
    {
        string name = String.IsNullOrEmpty(_customInputField.Text) ? GlobalFunctions.RedColorText("N/A") : GlobalFunctions.BlueColorText(_customInputField.Text);
        return Keys.RoomName + name + "\n";
    }

    public void StoreData(MatchmakeData matchmakeData)
    {
        string roomName = _customInputField.Text;
        matchmakeData.IsRoomNameSet = !String.IsNullOrEmpty(roomName);
        matchmakeData.RoomName = roomName;
    }
}
