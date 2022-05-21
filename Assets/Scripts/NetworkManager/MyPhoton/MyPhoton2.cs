﻿using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public partial class MyPhoton : MonoBehaviour
{
    public static void CreateRoom(string roomName, string password, bool isPasswordSet)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CleanupCacheOnLeave = true;
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

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
