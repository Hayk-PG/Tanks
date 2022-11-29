using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public partial class MyPhoton : MonoBehaviour
{
    public static void Connect(string nickName, string userId)
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.NickName = nickName;
            PhotonNetwork.AuthValues = new AuthenticationValues { UserId = userId };
        }
    }

    public static void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }
}
