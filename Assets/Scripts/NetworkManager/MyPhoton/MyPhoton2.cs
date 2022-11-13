using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public partial class MyPhoton : MonoBehaviour
{
    private static TypedLobby LobbyType(string lobbyName, LobbyType lobbyType)
    {
        return new TypedLobby(lobbyName, lobbyType);
    }

    public static void CreateRoom(LobbyType lobbyType, string lobbyName, MatchmakeData matchmakeData)
    {
        DefineRoomOptions(out RoomOptions roomOptions);
        SetRoomCustomProperties(roomOptions, matchmakeData);
        PhotonNetwork.CreateRoom(matchmakeData.RoomName, roomOptions, LobbyType(lobbyName, lobbyType));
    }

    private static void DefineRoomOptions(out RoomOptions roomOptions)
    {
        roomOptions = new RoomOptions();
        roomOptions.CleanupCacheOnLeave = true;
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
    }

    private static void SetRoomCustomProperties(RoomOptions roomOptions, MatchmakeData matchmakeData)
    {
        Hashtable _customProperties = new Hashtable();
        roomOptions.CustomRoomPropertiesForLobby = new string[3] { Keys.MapIndex, Keys.GameTime, Keys.MapWind };

        _customProperties.Add(Keys.MapIndex, matchmakeData.MapIndex);
        _customProperties.Add(Keys.GameTime, matchmakeData.Time);
        _customProperties.Add(Keys.MapWind, matchmakeData.IsWindOn);

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
