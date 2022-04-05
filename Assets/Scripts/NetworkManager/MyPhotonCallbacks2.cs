using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;

public partial class MyPhotonCallbacks : MonoBehaviourPunCallbacks
{
    public Action<Room> OnRoomCreated { get; set; }
    public Action<short, string> OnRoomFailedToCreate { get; set; }
    public Action<RoomInfo> OnUpdateRoomList { get; set; }
    public Action<Room> OnRoomJoined { get; set; }
    public Action<Player> OnRoomPlayerEntered { get; set; }
    public Action<Player> OnRoomPlayerLeft { get; set; }
    public Action<short, string> OnRoomFailedToJoin { get; set; }



    public override void OnCreatedRoom()
    {
        OnRoomCreated?.Invoke(PhotonNetwork.CurrentRoom);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        OnRoomFailedToCreate?.Invoke(returnCode, message);
    }

    public override void OnJoinedRoom()
    {
        OnRoomJoined?.Invoke(PhotonNetwork.CurrentRoom);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnRoomPlayerEntered?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnRoomPlayerLeft?.Invoke(otherPlayer);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        OnRoomFailedToJoin?.Invoke(returnCode, message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            OnUpdateRoomList?.Invoke(roomList[i]);
        }
    }
}
