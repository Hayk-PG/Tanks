using UnityEngine;
using Photon.Pun;

public partial class MyPhoton: MonoBehaviour
{
    public static void LeaveRoom()
    {
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
    }

    public static void LeaveLobby()
    {
        if (PhotonNetwork.InLobby) PhotonNetwork.LeaveLobby();
    }
}
