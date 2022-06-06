using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public partial class MyPhoton : MonoBehaviour
{
    public static void CreateRoom(string roomName, string password, bool isPasswordSet, int mapIndex)
    {
        RoomOptions roomOptions = new RoomOptions();
        ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CleanupCacheOnLeave = true;
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        roomOptions.CustomRoomPropertiesForLobby = new string[1] { Keys.MapIndex};
        _customProperties.Add(Keys.MapIndex, mapIndex);
        roomOptions.CustomRoomProperties = _customProperties;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public static void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public static void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }
}
