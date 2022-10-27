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
        DefineRoomOptions(out RoomOptions roomOptions);
        SetRoomCustomProperties(roomOptions, new object[] { mapIndex, gameTime , isWindOn });
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, LobbyType(lobbyName, lobbyType));
    }

    private static void DefineRoomOptions(out RoomOptions roomOptions)
    {
        roomOptions = new RoomOptions();
        roomOptions.CleanupCacheOnLeave = true;
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
    }

    private static void SetRoomCustomProperties(RoomOptions roomOptions, object[] roomCustomProperties)
    {
        ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CustomRoomPropertiesForLobby = new string[3] { Keys.MapIndex, Keys.GameTime, Keys.MapWind };

        _customProperties.Add(Keys.MapIndex, roomCustomProperties[0]);
        _customProperties.Add(Keys.GameTime, roomCustomProperties[1]);
        _customProperties.Add(Keys.MapWind, roomCustomProperties[2]);

        roomOptions.CustomRoomProperties = _customProperties;
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
