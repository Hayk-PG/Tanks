using System;

public class MatchmakeRoomName : MatchmakeRoomInfo
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
}
