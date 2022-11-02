using System;


public class MatchmakeRoomPassword : MatchmakeRoomInfo
{
    public override string TextResultOffline()
    {
        return "\n";
    }

    public override string TextResultOnline()
    {
        string password = String.IsNullOrEmpty(_customInputField.Text) ? GlobalFunctions.RedColorText("N/A") : GlobalFunctions.BlueColorText(_customInputField.Text);
        return Keys.RoomPassword + password + "\n";
    }
}
