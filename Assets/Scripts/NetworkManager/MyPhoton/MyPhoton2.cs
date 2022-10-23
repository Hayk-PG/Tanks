using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public partial class MyPhoton : MonoBehaviour
{
    private static TypedLobby LobbyType(string lobbyName, LobbyType lobbyType)
    {
        return new TypedLobby(lobbyName, lobbyType);
    }

    public static void CreateRoom(LobbyType lobbyType, string lobbyName, string roomName, string password, bool isPasswordSet, int mapIndex, int gameTime, bool isWindOn)
    {
        RoomOptions roomOptions = new RoomOptions();      
        roomOptions.CleanupCacheOnLeave = true;
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CustomRoomPropertiesForLobby = new string[3] { Keys.MapIndex, Keys.GameTime, Keys.MapWind};

        _customProperties.Add(Keys.MapIndex, mapIndex);
        _customProperties.Add(Keys.GameTime, gameTime);
        _customProperties.Add(Keys.MapWind, isWindOn); 
        
        roomOptions.CustomRoomProperties = _customProperties;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, LobbyType(lobbyName, lobbyType));
    }

    public static void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public static void JoinLobby(string lobbyName, LobbyType lobbyType)
    {
        PhotonNetwork.JoinLobby(LobbyType(lobbyName, lobbyType));
    }
}
