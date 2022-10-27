using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;

public partial class MyPhotonCallbacks : MonoBehaviourPunCallbacks
{
    public Action<Room> _OnCreatedRoom { get; set; }
    public Action<short, string> _OnCreateRoomFailed { get; set; }   
    public Action<Room> _OnJoinedRoom { get; set; }
    public Action<Player> _OnPlayerEnteredRoom { get; set; }   
    public Action<short, string> _OnJoinRoomFailed { get; set; }

    public event Action<RoomInfo> onRoomListUpdate;


    public override void OnCreatedRoom()
    {
        _OnCreatedRoom?.Invoke(PhotonNetwork.CurrentRoom);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _OnCreateRoomFailed?.Invoke(returnCode, message);
    }

    public override void OnJoinedRoom()
    {
        _OnJoinedRoom?.Invoke(PhotonNetwork.CurrentRoom);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _OnPlayerEnteredRoom?.Invoke(newPlayer);
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _OnJoinRoomFailed?.Invoke(returnCode, message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            onRoomListUpdate?.Invoke(roomList[i]);
        }
    }
}
