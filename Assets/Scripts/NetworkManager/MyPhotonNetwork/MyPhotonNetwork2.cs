using Photon.Pun;
using Photon.Realtime;

public partial class MyPhotonNetwork 
{
    public static Room CurrentRoom => PhotonNetwork.CurrentRoom;
    public static TypedLobby CurrentLobby => PhotonNetwork.CurrentLobby;
    public static bool IsInRoom => PhotonNetwork.InRoom;
    public static bool IsInLobby => PhotonNetwork.InLobby;


    public static bool IsDesiredLobby(LobbyType lobbyType, string lobbyName)
    {
        return CurrentLobby.Type == lobbyType && CurrentLobby.Name == lobbyName;
    }

    public static void LoadLevel()
    {
        PhotonNetwork.LoadLevel(MyScene.Manager.GameSceneName);
    }   
}
